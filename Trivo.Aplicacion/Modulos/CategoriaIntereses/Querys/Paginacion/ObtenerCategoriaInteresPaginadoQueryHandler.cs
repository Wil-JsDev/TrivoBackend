using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using Trivo.Aplicacion.Abstracciones.Mensajes;
using Trivo.Aplicacion.DTOs.CategoriaIntereses;
using Trivo.Aplicacion.Interfaces.Repositorio;
using Trivo.Aplicacion.Paginacion;
using Trivo.Aplicacion.Utilidades;

namespace Trivo.Aplicacion.Modulos.CategoriaIntereses.Querys.Paginacion;

internal sealed class ObtenerCategoriaInteresPaginadoQueryHandler(
    ILogger<ObtenerCategoriaInteresPaginadoQueryHandler> logger,
    IDistributedCache cache,
    IRepositorioCategoriaInteres repositorioCategoriaInteres
    ) : IQueryHandler<ObtenerCategoriaInteresPaginadoQuery, ResultadoPaginado<CategoriaInteresDto>>
{
    public async Task<ResultadoT<ResultadoPaginado<CategoriaInteresDto>>> Handle(
        ObtenerCategoriaInteresPaginadoQuery request, 
        CancellationToken cancellationToken
    )
    {
        if (request is null)
        {
            logger.LogWarning("La solicitud para obtener intereses por categoría llegó como null.");
        
            return ResultadoT<ResultadoPaginado<CategoriaInteresDto>>.Fallo(
                Error.Fallo("400", "La solicitud no puede ser nula."));
        }
        
        if (request.NumeroPagina <= 0 || request.TamanoPagina <= 0)
        {
            logger.LogWarning("Los parámetros de paginación son inválidos. NúmeroPagina: {NumeroPagina}, TamanoPagina: {TamanoPagina}", 
                request.NumeroPagina, request.TamanoPagina);
            return ResultadoT<ResultadoPaginado<CategoriaInteresDto>>.Fallo(
                Error.Conflicto("409", "Los parámetros de paginación deben ser mayores a cero."));
        }
        
        var resultadoPagina = await cache.ObtenerOCrearAsync($"obtener-categoria-intereses-paginado-{request.NumeroPagina}-{request.TamanoPagina}",
            async () => await repositorioCategoriaInteres.ObtenerCategoriaInteresPaginadoAsync( 
                request.NumeroPagina, 
                request.TamanoPagina, 
                cancellationToken), cancellationToken: cancellationToken);
        
        var dtoList = resultadoPagina.Elementos!.Select(ci => new CategoriaInteresDto
        (
            CategoriaInteresId: ci.CategoriaId ?? Guid.Empty,
            Nombre: ci.Nombre!
        ));
        
        var resultadoPaginado = new ResultadoPaginado<CategoriaInteresDto>(
            elementos: dtoList,
            totalElementos: resultadoPagina.TotalElementos,
            paginaActual: request.NumeroPagina,
            tamanioPagina: request.TamanoPagina
        );
        
        logger.LogInformation("");
        
        return ResultadoT<ResultadoPaginado<CategoriaInteresDto>>.Exito(resultadoPaginado);
    }
    
}