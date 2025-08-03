using System.Text.RegularExpressions;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using Trivo.Aplicacion.Abstracciones.Mensajes;
using Trivo.Aplicacion.DTOs.Cuentas.Expertos;
using Trivo.Aplicacion.DTOs.Cuentas.Reclutador;
using Trivo.Aplicacion.DTOs.Cuentas.Usuarios;
using Trivo.Aplicacion.DTOs.Habilidades;
using Trivo.Aplicacion.DTOs.Intereses;
using Trivo.Aplicacion.Interfaces.Repositorio.Cuenta;
using Trivo.Aplicacion.Interfaces.Servicios.IA;
using Trivo.Aplicacion.Interfaces.Servicios.SignaIR;
using Trivo.Aplicacion.Mapper;
using Trivo.Aplicacion.Paginacion;
using Trivo.Aplicacion.Utilidades;
using Trivo.Dominio.Enum;

namespace Trivo.Aplicacion.Modulos.Usuario.Querys.ObtenerRecomendacionUsuarios;

internal sealed class RecomendacionUsuariosQueryHandler(
    ILogger<RecomendacionUsuariosQueryHandler> logger,
    IRepositorioUsuario repositorioUsuario,
    IDistributedCache cache,
    IOllamaServicio ollamaServicio,
    INotificadorIA notificadorIa,
    IRepositorioExperto repositorioExperto,
    IRepositorioReclutador repositorioReclutador
    ) : IQueryHandler<RecomendacionUsuariosQuery, ResultadoPaginado<UsuarioRecomendacionIaDto>>
{
    public async Task<ResultadoT<ResultadoPaginado<UsuarioRecomendacionIaDto>>> Handle(RecomendacionUsuariosQuery request, CancellationToken cancellationToken)
    {
        var usuarioActual = await repositorioUsuario.ObtenerUsuarioConInteresYHabilidades(request.UsuarioId, cancellationToken);
        
        if (usuarioActual is null) 
        {
            logger.LogWarning("No se encontró el usuario con Id {UsuarioId} o no tiene intereses/habilidades registrados.", request.UsuarioId);
            
            return ResultadoT<ResultadoPaginado<UsuarioRecomendacionIaDto>>.Fallo(Error.Fallo("404", "El usuario no fue encontrado o no tiene datos de intereses/habilidades."));
        }

        var rol = await repositorioUsuario.ObtenerRolDeUsuarioAsync(usuarioActual.Id ?? Guid.Empty, cancellationToken);
        
        var usuariosParaComparar = await repositorioUsuario.ObtenerUsuariosObjetivoAsync(
            usuarioActual.Id ?? Guid.Empty,
            rol,
            cancellationToken);
        
        IEnumerable<Dominio.Modelos.Usuario> paraComparar = usuariosParaComparar.ToList();

        var prompt = ConstruirPrompt(usuarioActual, paraComparar.ToList(), rol);
        
        logger.LogInformation("Prompt construido correctamente para el usuario {UsuarioId}. Enviando solicitud a la IA...", usuarioActual.Id);
        
        var respuestaIa = await cache.ObtenerOCrearAsync($"respuesta-ia-{usuarioActual.Id}",
            async () => await ollamaServicio.EnviarPeticionIaAsync("tinyllama:1.1b", prompt),
            cancellationToken: cancellationToken
        );
        
        if (string.IsNullOrWhiteSpace(respuestaIa))
        {
            logger.LogWarning("La IA devolvió una respuesta vacía o nula para el usuario {UsuarioId}.", usuarioActual.Id);
            
            return ResultadoT<ResultadoPaginado<UsuarioRecomendacionIaDto>>.Fallo(Error.Fallo("500", "La IA devolvió una respuesta vacía o inválida."));
        }
        
        logger.LogWarning("Respuesta cruda IA:\n{RespuestaIa}", respuestaIa);

        var guidRegex = new Regex(@"\b[0-9a-fA-F]{8}\b-[0-9a-fA-F]{4}\b-[0-9a-fA-F]{4}\b-[0-9a-fA-F]{4}\b-[0-9a-fA-F]{12}\b");
        var matches = guidRegex.Matches(respuestaIa);

        var idsRecomendados = matches
            .Select(m => Guid.TryParse(m.Value, out var guid) ? guid : Guid.Empty)
            .Where(g => g != Guid.Empty)
            .Distinct()
            .ToList();

        var usuariosRecomendados = paraComparar
            .Where(u => u.Id.HasValue && idsRecomendados.Contains(u.Id.Value))
            .ToList();

        if (!usuariosRecomendados.Any())
        {
            logger.LogWarning("La IA no devolvió IDs válidos. Aplicando lógica de respaldo basada en similitud de intereses/habilidades.");
            
            usuariosRecomendados = ObtenerUsuariosSimilares(usuarioActual, paraComparar.ToList());
        }

        if ( await repositorioExperto.EsUsuarioExpertoAsync(request.UsuarioId, cancellationToken) )
        {
            var experto = await repositorioExperto.ObtenerDetallesExpertoAsync(request.UsuarioId, cancellationToken);

            var expertoReconmendando = usuariosRecomendados
                .Select(usuario => RecomendacionMapper.MappearAExpertoDto(usuario, experto))
                .Paginar(request.NumeroPagina, request.TamanoPagina)
                .ToList();

            var total = expertoReconmendando.Count();
            
            ResultadoPaginado<UsuarioRecomendacionIaDto> resultadoPaginadoExperto = new
            (
                expertoReconmendando,
                total,
                request.NumeroPagina,
                request.TamanoPagina
            );
            
            return ResultadoT<ResultadoPaginado<UsuarioRecomendacionIaDto>>.Exito(resultadoPaginadoExperto);
        }

        if ( await repositorioReclutador.EsUsuarioReclutadorAsync(request.UsuarioId, cancellationToken) )
        {
            var reclutador = await repositorioReclutador.ObtenerDetallesReclutadorAsync(request.UsuarioId, cancellationToken);

            var reclutadorReconmendando = usuariosRecomendados
                .Select(usuario => RecomendacionMapper.MappearAReclutadorDto(usuario, reclutador!))
                .Paginar(request.NumeroPagina, request.TamanoPagina)
                .ToList();

            var total = reclutadorReconmendando.Count();
            
            ResultadoPaginado<UsuarioRecomendacionIaDto> resultadoPaginadoExperto = new
            (
                reclutadorReconmendando,
                total,
                request.NumeroPagina,
                request.TamanoPagina
            );
            
            return ResultadoT<ResultadoPaginado<UsuarioRecomendacionIaDto>>.Exito(resultadoPaginadoExperto);
        }
        
        // Caso por defecto de no ser experto/reclutador
        var usuarioRecomendacionIa = usuariosRecomendados
            .Select(UsuarioMapper.MappearRecomendacionIaDto)
            .Paginar(request.NumeroPagina, request.TamanoPagina)
            .ToList();

        var totalElementos = usuarioRecomendacionIa.Count();
        
        ResultadoPaginado<UsuarioRecomendacionIaDto> resultadoPaginado = new
        (
            usuarioRecomendacionIa,
            totalElementos,
            request.NumeroPagina,
            request.TamanoPagina
        );
        
        await notificadorIa.NotificarRecomendaciones(usuarioActual.Id ?? Guid.Empty, resultadoPaginado.Elementos!.ToList());
        
        return ResultadoT<ResultadoPaginado<UsuarioRecomendacionIaDto>>.Exito(resultadoPaginado);
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
       private string ConstruirPrompt(Dominio.Modelos.Usuario usuarioActual, List<Dominio.Modelos.Usuario> usuarios, string rolUsuario)
        {
                var interesesUsuario = string.Join(", ", usuarioActual.UsuarioInteres?.Select(i => i.Interes?.Nombre).Where(n => !string.IsNullOrWhiteSpace(n)) ?? Array.Empty<string>());
                var habilidadesUsuario = string.Join(", ", usuarioActual.UsuarioHabilidades?.Select(h => h.Habilidad?.Nombre).Where(n => !string.IsNullOrWhiteSpace(n)) ?? Array.Empty<string>());

                var usuariosContexto = usuarios.Select((u) =>
                {
                    var intereses = string.Join(", ", u.UsuarioInteres?.Select(i => i.Interes?.Nombre).Where(n => !string.IsNullOrWhiteSpace(n)) ?? Array.Empty<string>());
                    var habilidades = string.Join(", ", u.UsuarioHabilidades?.Select(h => h.Habilidad?.Nombre).Where(n => !string.IsNullOrWhiteSpace(n)) ?? Array.Empty<string>());
                    return $"{u.Id}: {intereses} | {habilidades}";
                });

                var tipoRecomendacion = rolUsuario == nameof(Roles.Reclutador) ? "EXPERTOS" : 
                                      rolUsuario == nameof(Roles.Experto) ? "RECLUTADORES" : 
                                      "usuarios relevantes";

                var prompt = $@"
                ## CONTEXTO DE RECOMENDACIÓN ##
                { (rolUsuario == nameof(Roles.Reclutador) ? 
                    "Eres un RECLUTADOR buscando EXPERTOS que coincidan con estos requisitos:" : 
                    rolUsuario == nameof(Roles.Experto) ? 
                    "Eres un EXPERTO buscando RECLUTADORES interesados en tu perfil:" : 
                    "Busca usuarios con intereses/habilidades similares:")}

                ## TU PERFIL ##
                Intereses: {interesesUsuario}
                Habilidades: {habilidadesUsuario}

                ## CANDIDATOS DISPONIBLES ##
                {string.Join("\n", usuariosContexto)}

                ## INSTRUCCIONES ESTRICTAS ##
                1. FILTRO: Seleccionar exclusivamente {tipoRecomendacion}
                2. CRITERIOS: Evaluar compatibilidad basada en:
                   - Coincidencia de intereses (prioridad alta)
                   - Complementariedad de habilidades (prioridad media)
                3. CANTIDAD: Seleccionar exactamente los 9 mejores candidatos
                4. FORMATO: Responder ÚNICAMENTE con los IDs válidos en formato:
                   id1,id2,id3,...,id9

                ## EJEMPLO VÁLIDO ##
                123e4567-e89b-12d3-a456-426614174000,987e6543-e21b-12d3-a456-426614175000,...,9a8b7c6d-5e4f-3g2h-1i0j-426614175000

                ## TU RESPUESTA (SOLO IDs) ##
                ";

                return prompt.Trim();
        }
        
    #endregion
}