using System.Text.RegularExpressions;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using Trivo.Aplicacion.Abstracciones.Mensajes;
using Trivo.Aplicacion.DTOs.Cuentas.Usuarios;
using Trivo.Aplicacion.Interfaces.Repositorio.Cuenta;
using Trivo.Aplicacion.Interfaces.Servicios.IA;
using Trivo.Aplicacion.Interfaces.Servicios.SignaIR;
using Trivo.Aplicacion.Mapper;
using Trivo.Aplicacion.Paginacion;
using Trivo.Aplicacion.Utilidades;

namespace Trivo.Aplicacion.Modulos.Usuario.Querys.ObtenerRecomendacionUsuarios;

internal sealed class RecomendacionUsuariosQueryHandler(
    ILogger<RecomendacionUsuariosQueryHandler> logger,
    IRepositorioUsuario repositorioUsuario,
    IDistributedCache cache,
    IOllamaServicio ollamaServicio,
    INotificadorIA notificadorIa
    ) : IQueryHandler<RecomendacionUsuariosQuery, ResultadoPaginado<UsuarioReconmendacionDto>>
{
    public async Task<ResultadoT<ResultadoPaginado<UsuarioReconmendacionDto>>> Handle(RecomendacionUsuariosQuery request, CancellationToken cancellationToken)
    {
        var usuarioActual = await repositorioUsuario.ObtenerUsuarioConInteresYHabilidades(request.UsuarioId, cancellationToken);
        
        if (usuarioActual is null) 
        {
            logger.LogWarning("No se encontró el usuario con Id {UsuarioId} o no tiene intereses/habilidades registrados.", request.UsuarioId);
            
            return ResultadoT<ResultadoPaginado<UsuarioReconmendacionDto>>.Fallo(Error.Fallo("404", "El usuario no fue encontrado o no tiene datos de intereses/habilidades."));
        }

        var rol = await repositorioUsuario.ObtenerRolDeUsuarioAsync(usuarioActual.Id ?? Guid.Empty, cancellationToken);
        
        var usuariosParaComparar = await repositorioUsuario.ObtenerUsuariosObjetivoAsync(usuarioActual.Id ?? Guid.Empty, rol, cancellationToken);
        
        IEnumerable<Dominio.Modelos.Usuario> paraComparar = usuariosParaComparar.ToList();
        
        var prompt = ConstruirPrompt(usuarioActual, paraComparar.ToList());
        
        logger.LogInformation("Prompt construido correctamente para el usuario {UsuarioId}. Enviando solicitud a la IA...", usuarioActual.Id);
        
        var respuestaIa = await cache.ObtenerOCrearAsync($"respuesta-ia-{usuarioActual.Id}",
            async () => await ollamaServicio.EnviarPeticionIaAsync("tinyllama:1.1b", prompt),
            cancellationToken: cancellationToken
        );
        
        if (string.IsNullOrWhiteSpace(respuestaIa))
        {
            logger.LogWarning("La IA devolvió una respuesta vacía o nula para el usuario {UsuarioId}.", usuarioActual.Id);
            
            return ResultadoT<ResultadoPaginado<UsuarioReconmendacionDto>>.Fallo(Error.Fallo("500", "La IA devolvió una respuesta vacía o inválida."));
        }
        
        // Extraer índices con Regex para evitar problemas con el texto adicional de la respuesta
        var indicesRecomendados = Regex.Matches(respuestaIa, @"\d+")
            .Select(m => int.Parse(m.Value) - 1) // índice 0-based
            .Where(idx => idx >= 0 && idx < paraComparar.Count())
            .Distinct()
            .ToList();

        var usuariosRecomendados = indicesRecomendados
            .Select(idx => paraComparar.ElementAt(idx))
            .ToList();
        
        if (!usuariosRecomendados.Any())
        {
            logger.LogWarning("La IA no devolvió nombres válidos. Aplicando lógica de respaldo basada en similitud de intereses/habilidades.");

            usuariosRecomendados = ObtenerUsuariosSimilares(usuarioActual, paraComparar.ToList());
        }
        
        logger.LogWarning("Respuesta cruda IA: {Respuesta}", respuestaIa);
        
        var cacheKey = $"recomendaciones-ia-dto-{request.UsuarioId}-pag-{request.NumeroPagina}-size-{request.TamanoPagina}";

        var resultadoPaginado = await cache.ObtenerOCrearAsync(
            cacheKey,
            async () =>
            {
                var usuarioReconmendandosDtos = usuariosRecomendados
                    .Select(UsuarioMapper.MapToDto)
                    .ToList();

                var totalElementos = usuarioReconmendandosDtos.Count;

                var elementosPaginados = usuarioReconmendandosDtos
                    .Paginar(request.NumeroPagina, request.TamanoPagina)
                    .ToList();

                return new ResultadoPaginado<UsuarioReconmendacionDto>(
                    elementosPaginados,
                    totalElementos,
                    request.NumeroPagina,
                    request.TamanoPagina
                );
            },
            cancellationToken: cancellationToken
        );
        
        logger.LogInformation("Se obtuvieron {Cantidad} recomendaciones para el usuario {UsuarioId}.", resultadoPaginado.Elementos!.Count(), usuarioActual.Id);
        
        await notificadorIa.NotificarRecomendaciones(usuarioActual.Id ?? Guid.Empty, resultadoPaginado.Elementos!.ToList());
        
        return ResultadoT<ResultadoPaginado<UsuarioReconmendacionDto>>.Exito(resultadoPaginado);
    }

    #region Metodos privados
        private List<Dominio.Modelos.Usuario> ObtenerUsuariosSimilares(Dominio.Modelos.Usuario usuarioActual, List<Dominio.Modelos.Usuario> usuarios, int topN = 9)
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
            - Devuelve SOLO los números (índices) de los 9 usuarios MÁS SIMILARES.
            - La respuesta debe ser ÚNICAMENTE una lista de números separados por comas, SIN espacios, SIN texto adicional, SIN explicaciones.
            - Ejemplo de respuesta válida: 1,3,2

            Respuesta:
            ";

            return prompt.Trim();
        }
    #endregion
}