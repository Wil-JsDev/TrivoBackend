using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using Trivo.Aplicacion.Abstracciones.Mensajes;
using Trivo.Aplicacion.DTOs.Habilidades;
using Trivo.Aplicacion.Interfaces.Repositorio;
using Trivo.Aplicacion.Paginacion;
using Trivo.Aplicacion.Utilidades;

namespace Trivo.Aplicacion.Modulos.Habilidades.Querys.Paginacion;

internal sealed class ObtenerPaginasHabilidadesQueryHandler(
    ILogger<ObtenerPaginasHabilidadesQueryHandler> logger,
    IRepositorioHabilidad habilidadRepositorio,
    IDistributedCache cache
    ) : IQueryHandler<ObtenerPaginasHabilidadesQuery, ResultadoPaginado<HabilidadDto>>
{
        public async Task<ResultadoT<ResultadoPaginado<HabilidadDto>>> Handle(ObtenerPaginasHabilidadesQuery request, CancellationToken cancellationToken)
    {
        if (request is null)
        {
            logger.LogWarning("La solicitud para obtener habilidades paginadas fue nula.");
            
            return ResultadoT<ResultadoPaginado<HabilidadDto>>.Fallo(
                Error.Fallo("400", "La solicitud no puede ser nula."));
        }

        if (request.NumeroPagina <= 0 || request.TamanoPagina <= 0)
        {
            logger.LogWarning("Parámetros de paginación inválidos. NumeroPagina={NumeroPagina}, TamanoPagina={TamanoPagina}", 
                request.NumeroPagina, request.TamanoPagina);

            return ResultadoT<ResultadoPaginado<HabilidadDto>>.Fallo(
                Error.Fallo("400", "Los parámetros de paginación deben ser mayores a cero."));
        }

        var resultadoPaginado = await cache.ObtenerOCrearAsync(
            $"obtener-paginas-habilidades-{request.NumeroPagina}-{request.TamanoPagina}",
            async () => await habilidadRepositorio.ObtenerHabilidadPaginadoAsync(
                request.NumeroPagina,
                request.TamanoPagina,
                cancellationToken),
            cancellationToken: cancellationToken
        );

        var habilidadesElementos = resultadoPaginado.Elementos!
            .Select(x => new HabilidadDto(
                x.HabilidadId,
                x.Nombre!,
                x.FechaRegistro
            ))
            .ToList();

        if (!habilidadesElementos.Any())
        {
            logger.LogWarning("No se encontraron habilidades en la página {NumeroPagina}.", request.NumeroPagina);
            
            return ResultadoT<ResultadoPaginado<HabilidadDto>>.Fallo(
                Error.Fallo("404", "No se encontraron habilidades."));
        }

        var resultado = new ResultadoPaginado<HabilidadDto>(
            elementos: habilidadesElementos,
            totalElementos: resultadoPaginado.TotalElementos,
            paginaActual: request.NumeroPagina,
            tamanioPagina: request.TamanoPagina
        );

        logger.LogInformation("Página {NumeroPagina} de habilidades obtenida exitosamente. Total de elementos: {Cantidad}",
            request.NumeroPagina, habilidadesElementos.Count);

        return ResultadoT<ResultadoPaginado<HabilidadDto>>.Exito(resultado);
    }

}