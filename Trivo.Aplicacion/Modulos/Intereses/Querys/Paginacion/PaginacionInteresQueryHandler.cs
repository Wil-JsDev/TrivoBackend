using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using Trivo.Aplicacion.Abstracciones.Mensajes;
using Trivo.Aplicacion.DTOs.Intereses;
using Trivo.Aplicacion.Interfaces.Repositorio;
using Trivo.Aplicacion.Paginacion;
using Trivo.Aplicacion.Utilidades;

namespace Trivo.Aplicacion.Modulos.Intereses.Querys.Paginacion;

internal sealed class PaginacionInteresQueryHandler(
    ILogger<PaginacionInteresQueryHandler> logger,
    IRepositorioInteres repositorioInteres,
    IDistributedCache cache
    ) : IQueryHandler<PaginacionInteresQuery, ResultadoPaginado<InteresDto>>
{
    public async Task<ResultadoT<ResultadoPaginado<InteresDto>>> Handle(PaginacionInteresQuery request, CancellationToken cancellationToken)
    {
        if (request.NumeroPagina <= 0 || request.TamanoPagina <= 0)
        {
            logger.LogWarning("PaginacionInteresQuery recibió parámetros inválidos: NúmeroPagina={NumeroPagina}, TamanoPagina={TamanoPagina}",
                request.NumeroPagina, request.TamanoPagina);

            return ResultadoT<ResultadoPaginado<InteresDto>>.Fallo(
                Error.Fallo("400", "Los parámetros de paginación deben ser mayores a cero."));
        }

        var resultadoPaginado = await cache.ObtenerOCrearAsync(
            $"intereses-paginados-{request.NumeroPagina}-{request.TamanoPagina}",
            async () =>
            {
                var resultadoPaginacion = await repositorioInteres.ObtenerInteresesPaginadosAsync(
                    request.NumeroPagina,
                    request.TamanoPagina,
                    cancellationToken);

                var resultadoPaginacionDto = resultadoPaginacion.Elementos!.Select(x => new InteresDto
                (
                    InteresId: x.Id ?? Guid.Empty,
                    Nombre: x.Nombre
                )).ToList();

                var total = resultadoPaginacionDto.Count;

                return new ResultadoPaginado<InteresDto>(
                    elementos: resultadoPaginacionDto,
                    totalElementos: total,
                    paginaActual: request.NumeroPagina,
                    tamanioPagina: request.TamanoPagina
                );
            },
            cancellationToken: cancellationToken
        );

        logger.LogInformation("Se obtuvo la paginación de intereses correctamente. Página {NumeroPagina}, Tamaño {TamanoPagina}, TotalElementos={Total}",
            request.NumeroPagina, request.TamanoPagina, resultadoPaginado.TotalElementos);

        return ResultadoT<ResultadoPaginado<InteresDto>>.Exito(resultadoPaginado);
    }

}