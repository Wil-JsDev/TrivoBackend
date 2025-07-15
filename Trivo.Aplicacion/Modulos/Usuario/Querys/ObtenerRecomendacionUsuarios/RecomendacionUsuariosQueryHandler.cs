using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using Trivo.Aplicacion.Abstracciones.Mensajes;
using Trivo.Aplicacion.DTOs.Cuentas.Usuarios;
using Trivo.Aplicacion.DTOs.Habilidades;
using Trivo.Aplicacion.DTOs.Intereses;
using Trivo.Aplicacion.Interfaces.Repositorio.Cuenta;
using Trivo.Aplicacion.Interfaces.Servicios;
using Trivo.Aplicacion.Interfaces.Servicios.IA;
using Trivo.Aplicacion.Interfaces.Servicios.SignaIR;
using Trivo.Aplicacion.Mapper;
using Trivo.Aplicacion.Utilidades;

namespace Trivo.Aplicacion.Modulos.Usuario.Querys.ObtenerRecomendacionUsuarios;

internal sealed class RecomendacionUsuariosQueryHandler(
    ILogger<RecomendacionUsuariosQueryHandler> logger,
    IRepositorioUsuario repositorioUsuario,
    IDistributedCache cache,
    IOllamaServicio ollamaServicio,
    INotificadorIA notificadorIa
    ) : IQueryHandler<RecomendacionUsuariosQuery, IEnumerable<UsuarioReconmendacionDto>>
{
    public async Task<ResultadoT<IEnumerable<UsuarioReconmendacionDto>>> Handle(RecomendacionUsuariosQuery request, CancellationToken cancellationToken)
    {
        var usuarioActual = await repositorioUsuario.ObtenerUsuarioConInteresYHabilidades(request.UsuarioId, cancellationToken);
        
        if (usuarioActual is null) 
        {
            logger.LogWarning("No se encontró el usuario con Id {UsuarioId} o no tiene intereses/habilidades registrados.", request.UsuarioId);
            
            return ResultadoT<IEnumerable<UsuarioReconmendacionDto>>.Fallo(Error.Fallo("404", "El usuario no fue encontrado o no tiene datos de intereses/habilidades."));
        }

        var todosUsuarios = await repositorioUsuario.ObtenerTodosUsuariosConInteresesYHabilidades(cancellationToken);
        
        var usuariosParaComparar = todosUsuarios.Where(x => x.Id != usuarioActual.Id).ToList();  

        var prompt = ConstruirPrompt(usuarioActual, usuariosParaComparar);
        
        logger.LogInformation("Prompt construido correctamente para el usuario {UsuarioId}. Enviando solicitud a la IA...", usuarioActual.Id);
        
        var respuestaIa = await ollamaServicio.EnviarPeticionIaAsync("tinyllama:1.1b", prompt);
        
        if (string.IsNullOrWhiteSpace(respuestaIa))
        {
            logger.LogWarning("La IA devolvió una respuesta vacía o nula para el usuario {UsuarioId}.", usuarioActual.Id);
            
            return ResultadoT<IEnumerable<UsuarioReconmendacionDto>>.Fallo(Error.Fallo("500", "La IA devolvió una respuesta vacía o inválida."));
        }
        
        var indicesRecomendados = respuestaIa!
            .Split(',', StringSplitOptions.RemoveEmptyEntries)
            .Select(n => int.TryParse(n.Trim(), out var idx) ? idx - 1 : -1)
            .Where(idx => idx >= 0 && idx < usuariosParaComparar.Count)
            .Distinct()
            .ToList();

        var usuariosRecomendados = indicesRecomendados
            .Select(idx => usuariosParaComparar[idx])
            .ToList();

        if (!usuariosRecomendados.Any())
        {
            logger.LogWarning("La IA no devolvió nombres válidos. Aplicando lógica de respaldo basada en similitud de intereses/habilidades.");

            usuariosRecomendados = ObtenerUsuariosSimilares(usuarioActual, usuariosParaComparar, 5);
        }
        
        logger.LogWarning("Respuesta cruda IA: {Respuesta}", respuestaIa);
        
        var usuarioReconmendandosDto = usuariosRecomendados.Select(UsuarioMapper.MapToDto).ToList();
        
        await notificadorIa.NotificarRecomendaciones(usuarioActual.Id ?? Guid.Empty, usuarioReconmendandosDto);
        
        return ResultadoT<IEnumerable<UsuarioReconmendacionDto>>.Exito(usuarioReconmendandosDto);
    }

    #region Metodos privados
        private List<Dominio.Modelos.Usuario> ObtenerUsuariosSimilares(Dominio.Modelos.Usuario usuarioActual, List<Dominio.Modelos.Usuario> usuarios, int topN = 5)
        {
            int CalcularSimilitud(Dominio.Modelos.Usuario u)
            {
                int interesesSimilares = u.UsuarioInteres?.Count(i => usuarioActual.UsuarioInteres!.Any(ua => ua.InteresId == i.InteresId)) ?? 0;
                int habilidadesSimilares = u.UsuarioHabilidades?.Count(h => usuarioActual.UsuarioHabilidades!.Any(uh => uh.HabilidadId == h.HabilidadId)) ?? 0;
                return interesesSimilares + habilidadesSimilares;
            }

            return usuarios
                .Select(u => new { Usuario = u, Similitud = CalcularSimilitud(u) })
                .Where(x => x.Similitud > 0)
                .OrderByDescending(x => x.Similitud)
                .ThenBy(x => x.Usuario.Nombre)
                .Take(topN)
                .Select(x => x.Usuario)
                .ToList();
        }
        private string ConstruirPrompt(Dominio.Modelos.Usuario usuarioActual, List<Dominio.Modelos.Usuario> usuarios)
        {
            var interesesUsuario = string.Join(", ", usuarioActual.UsuarioInteres?.Select(i => i.Interes?.Nombre).Where(n => !string.IsNullOrWhiteSpace(n)) ?? Array.Empty<string>());
            var habilidadesUsuario = string.Join(", ", usuarioActual.UsuarioHabilidades?.Select(h => h.Habilidad?.Nombre).Where(n => !string.IsNullOrWhiteSpace(n)) ?? Array.Empty<string>());

            var usuariosContexto = usuarios.Select((u, i) =>
            {
                var intereses = string.Join(", ", u.UsuarioInteres?.Select(i => i.Interes?.Nombre).Where(n => !string.IsNullOrWhiteSpace(n)) ?? Array.Empty<string>());
                var habilidades = string.Join(", ", u.UsuarioHabilidades?.Select(h => h.Habilidad?.Nombre).Where(n => !string.IsNullOrWhiteSpace(n)) ?? Array.Empty<string>());
                return $"{i + 1} | Intereses: {intereses} | Habilidades: {habilidades}";
            });

            var prompt = $@"
                Eres un sistema que recomienda usuarios similares basado en intereses y habilidades.

                Usuario actual:
                - Intereses: {interesesUsuario}
                - Habilidades: {habilidadesUsuario}

                Usuarios disponibles:
                {string.Join("\n", usuariosContexto)}

                Instrucciones:
                - Devuelve SOLO los números (índices) de los 5 usuarios MÁS SIMILARES.
                - La respuesta debe ser ÚNICAMENTE una lista de números separados por comas, SIN espacios, SIN texto adicional, SIN explicaciones.
                - Ejemplo de respuesta válida: 1,3,2

                Respuesta:
                ";

            return prompt.Trim();
        }

    #endregion
}