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
        
        var resultadoPorRol = await ObtenerRecomendacionesPorRol(
            request.UsuarioId,
            usuariosRecomendados,
            request.NumeroPagina,
            request.TamanoPagina,
            cancellationToken);

        if (resultadoPorRol != null)
        {
            return resultadoPorRol;
        }

        // Caso por defecto de no ser experto/reclutador
        var usuarioRecomendacionIa = paraComparar
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
        private string ConstruirPrompt(Dominio.Modelos.Usuario usuarioActual, List<Dominio.Modelos.Usuario> usuarios)
        {
            var interesesUsuario = string.Join(", ", usuarioActual.UsuarioInteres?.Select(i => i.Interes?.Nombre).Where(n => !string.IsNullOrWhiteSpace(n)) ?? Array.Empty<string>());
            var habilidadesUsuario = string.Join(", ", usuarioActual.UsuarioHabilidades?.Select(h => h.Habilidad?.Nombre).Where(n => !string.IsNullOrWhiteSpace(n)) ?? Array.Empty<string>());

            var usuariosContexto = usuarios.Select((u) =>
            {
                var intereses = string.Join(", ", u.UsuarioInteres?.Select(i => i.Interes?.Nombre).Where(n => !string.IsNullOrWhiteSpace(n)) ?? Array.Empty<string>());
                var habilidades = string.Join(", ", u.UsuarioHabilidades?.Select(h => h.Habilidad?.Nombre).Where(n => !string.IsNullOrWhiteSpace(n)) ?? Array.Empty<string>());
                return $"{u.Id}: {intereses} | {habilidades}";
            });

            var prompt = $@"
            Recomienda los 9 usuarios más parecidos por intereses y habilidades.

            Usuario:
            {interesesUsuario} | {habilidadesUsuario}

            Candidatos:
            {string.Join("\n", usuariosContexto)}

            Devuelve SOLO los GUID de los más parecidos, separados por comas. NO EXPLIQUES NADA. NO DES TEXTOS. SOLO LOS ID.

            Ejemplo válido: 4ac34db1-5ce4-4631-b7ac-5afbeb03be02,a605b5bb-06ff-427b-b673-df4aafbb3277,...

            Tu respuesta:
            ";
            return prompt.Trim();
        }

        private async Task<ResultadoT<ResultadoPaginado<UsuarioRecomendacionIaDto>>> ObtenerRecomendacionesPorRol(
            Guid usuarioId,
            List<Dominio.Modelos.Usuario> usuariosRecomendados,
            int numeroPagina,
            int tamanoPagina,
            CancellationToken cancellationToken)
            {
                if (await repositorioExperto.EsUsuarioExpertoAsync(usuarioId, cancellationToken))
                {
                    return await ObtenerRecomendacionesEspecificas(
                        usuarioId,
                        usuariosRecomendados,
                        "experto",
                        RecomendacionMapper.MappearAExpertoDto,
                        async (id) => await repositorioExperto.ObtenerDetallesExpertoAsync(id, cancellationToken),
                        numeroPagina,
                        tamanoPagina,
                        cancellationToken);
                }
                        
                if (await repositorioReclutador.EsUsuarioReclutadorAsync(usuarioId, cancellationToken))
                {
                    return await ObtenerRecomendacionesEspecificas(
                        usuarioId,
                        usuariosRecomendados,
                        "reclutador",
                        RecomendacionMapper.MappearAReclutadorDto,
                        async (id) => await repositorioReclutador.ObtenerDetallesReclutadorAsync(id, cancellationToken),
                        numeroPagina,
                        tamanoPagina,
                        cancellationToken);
                }

                logger.LogWarning("El usuario {UsuarioId} no tiene un rol válido (experto/reclutador) para obtener recomendaciones especializadas", usuarioId);
                return ResultadoT<ResultadoPaginado<UsuarioRecomendacionIaDto>>.Fallo(
                    Error.Fallo(
                        "400", 
                        "El usuario no tiene un rol válido para recomendaciones especializadas. Se aplicarán recomendaciones genéricas."
                    )
                );
            }

            private async Task<ResultadoT<ResultadoPaginado<UsuarioRecomendacionIaDto>>> ObtenerRecomendacionesEspecificas(
                Guid usuarioId,
                List<Dominio.Modelos.Usuario> usuariosRecomendados,
                string tipoRol,
                Func<Dominio.Modelos.Usuario, UsuarioRecomendacionIaDto> mapper,
                Func<Guid, Task> obtenerDetalles,
                int numeroPagina,
                int tamanoPagina,
                CancellationToken cancellationToken)
            {
                var cacheKey = $"obtener-recomendacion-ia-{tipoRol}-usuario-id-{usuarioId}";

                var dto = await cache.ObtenerOCrearAsync(
                    cacheKey,
                    async () =>
                    {
                        await obtenerDetalles(usuarioId);

                        var recomendaciones = usuariosRecomendados
                            .Select(mapper)
                            .ToList();

                        return new ResultadoPaginado<UsuarioRecomendacionIaDto>(
                            recomendaciones,
                            recomendaciones.Count,
                            numeroPagina,
                            tamanoPagina
                        );
                    },
                    cancellationToken: cancellationToken
                );

                logger.LogInformation("Recomendaciones para {TipoRol} {UsuarioId} obtenidas correctamente", tipoRol, usuarioId);
                
                return ResultadoT<ResultadoPaginado<UsuarioRecomendacionIaDto>>.Exito(dto);
            }
        
        
    #endregion
}