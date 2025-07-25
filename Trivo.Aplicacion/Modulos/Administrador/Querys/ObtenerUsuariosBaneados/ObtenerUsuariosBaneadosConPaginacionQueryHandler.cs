using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using Trivo.Aplicacion.Abstracciones.Mensajes;
using Trivo.Aplicacion.DTOs.Reportes;
using Trivo.Aplicacion.Interfaces.Repositorio.Cuenta;
using Trivo.Aplicacion.Mapper;
using Trivo.Aplicacion.Paginacion;
using Trivo.Aplicacion.Utilidades;

namespace Trivo.Aplicacion.Modulos.Administrador.Querys.ObtenerUsuariosBaneados;

internal sealed class ObtenerUsuariosBaneadosConPaginacionQueryHandler(
    ILogger<ObtenerUsuariosBaneadosConPaginacionQueryHandler> logger,
    IRepositorioAdministrador repositorioAdministrador,
    IDistributedCache cache
    ) : IQueryHandler<ObtenerUsuariosBaneadosConPaginacionQuery, ResultadoPaginado<ReporteBanDto>>
{
    public async Task<ResultadoT<ResultadoPaginado<ReporteBanDto>>> Handle(
        ObtenerUsuariosBaneadosConPaginacionQuery request,
        CancellationToken cancellationToken)
    {
        if (request.NumeroPagina <= 0 || request.TamanoPagina <= 0)
        {
            logger.LogWarning("Parámetros inválidos de paginación: NúmeroPagina={NumeroPagina}, TamanoPagina={TamanoPagina}", request.NumeroPagina, request.TamanoPagina);
            
            return ResultadoT<ResultadoPaginado<ReporteBanDto>>.Fallo(Error.Fallo("400", "Los parámetros de paginación deben ser mayores que cero."));
        }

        var resultadoPaginado = await cache.ObtenerOCrearAsync(
            $"obtener-ultimos-usuarios-baneados-{request.NumeroPagina}-{request.TamanoPagina}",
            async () =>
            {
                var pagina = await repositorioAdministrador.ObtenerPaginadoUltimosBan(
                    request.NumeroPagina,
                    request.TamanoPagina,
                    cancellationToken
                );

                var resultadoPaginaDto = pagina.Elementos!
                    .Select(ReporteMapper.MapBanDto)
                    .ToList();
                
                logger.LogInformation("Se mapearon {Cantidad} reportes de usuarios baneados.", resultadoPaginaDto.Count);

                return new ResultadoPaginado<ReporteBanDto>(
                    elementos: resultadoPaginaDto,
                    totalElementos: pagina.TotalElementos,
                    paginaActual: request.NumeroPagina,
                    tamanioPagina: request.TamanoPagina
                );
            },
            cancellationToken: cancellationToken
        );

        if (!resultadoPaginado.Elementos!.Any())
        {
            logger.LogWarning(
                "No se encontraron usuarios baneados para la página {NumeroPagina} con tamaño {TamanoPagina}.",
                request.NumeroPagina,
                request.TamanoPagina);
            
            return ResultadoT<ResultadoPaginado<ReporteBanDto>>.Fallo(Error.Fallo("404", "No se encontraron usuarios baneados."));
        }
        
        logger.LogInformation(
            "Consulta exitosa: Se retornaron {Cantidad} usuarios baneados para la página {NumeroPagina}.",
            resultadoPaginado.Elementos!.Count(),
            request.NumeroPagina);
        
        return ResultadoT<ResultadoPaginado<ReporteBanDto>>.Exito(resultadoPaginado);
    }

}