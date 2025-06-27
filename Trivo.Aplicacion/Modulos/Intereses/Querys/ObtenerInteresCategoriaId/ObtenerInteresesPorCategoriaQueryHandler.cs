using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using Trivo.Aplicacion.Abstracciones.Mensajes;
using Trivo.Aplicacion.DTOs.Intereses;
using Trivo.Aplicacion.Interfaces.Repositorio;
using Trivo.Aplicacion.Paginacion;
using Trivo.Aplicacion.Utilidades;

namespace Trivo.Aplicacion.Modulos.Intereses.Querys.ObtenerInteresCategoriaId;

internal sealed class ObtenerInteresesPorCategoriaQueryHandler(
    ILogger<ObtenerInteresesPorCategoriaQueryHandler> logger,
    IRepositorioInteres repositorioInteres,
    IDistributedCache cache
    ) : IQueryHandler<ObtenerInteresesPorCategoriaQuery, ResultadoPaginado<InterestPorCategoriaIdDto>>
{
   public async Task<ResultadoT<ResultadoPaginado<InterestPorCategoriaIdDto>>> Handle(ObtenerInteresesPorCategoriaQuery request, CancellationToken cancellationToken)
    {
        if (request is null)
        {
            logger.LogWarning("La solicitud para obtener intereses por categoría llegó como null.");
            
            return ResultadoT<ResultadoPaginado<InterestPorCategoriaIdDto>>.Fallo(
                Error.Fallo("400", "La solicitud no puede ser nula."));
        }

        if (request.NumeroPagina <= 0 || request.TamanoPagina <= 0)
        {
            logger.LogWarning("Los parámetros de paginación son inválidos. NúmeroPagina: {NumeroPagina}, TamanoPagina: {TamanoPagina}", 
                request.NumeroPagina, request.TamanoPagina);
            return ResultadoT<ResultadoPaginado<InterestPorCategoriaIdDto>>.Fallo(
                Error.Conflicto("409", "Los parámetros de paginación deben ser mayores a cero."));
        }

        if (!request.CategoriaIds.Any())
        {
            logger.LogWarning("No se proporcionaron categorías para obtener los intereses.");
            return ResultadoT<ResultadoPaginado<InterestPorCategoriaIdDto>>.Fallo(
                Error.Fallo("400", "Debe proporcionar al menos una categoría."));
        }

        var resultadoPagina = await cache.ObtenerOCrearAsync($"obtener-intereses-por-categoria-{request.CategoriaIds}-{request.NumeroPagina}-{request.TamanoPagina}",
            async () => await repositorioInteres.ObtenerInteresPorICategoriaIdAsync(request.CategoriaIds, 
                request.NumeroPagina, 
                request.TamanoPagina, 
                cancellationToken), cancellationToken: cancellationToken);

        var dtoList = resultadoPagina.Elementos!.Select(i => new InterestPorCategoriaIdDto(
            InteresId: i.Id ?? Guid.Empty,
            Nombre: i.Nombre ?? string.Empty
        )).ToList();

        var resultadoPaginado = new ResultadoPaginado<InterestPorCategoriaIdDto>(
            elementos: dtoList,
            totalElementos: resultadoPagina.TotalElementos,
            paginaActual: request.NumeroPagina,
            tamanioPagina: request.TamanoPagina
        );

        logger.LogInformation("Intereses por categoría recuperados correctamente. Categorías: {CategoriaIds}, Total: {Total}",
            string.Join(", ", request.CategoriaIds), resultadoPaginado.TotalElementos);

        return ResultadoT<ResultadoPaginado<InterestPorCategoriaIdDto>>.Exito(resultadoPaginado);
    }
}