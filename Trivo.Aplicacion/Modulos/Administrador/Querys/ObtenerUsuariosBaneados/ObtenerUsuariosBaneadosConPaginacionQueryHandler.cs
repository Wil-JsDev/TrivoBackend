using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using Serilog;
using Trivo.Aplicacion.Abstracciones.Mensajes;
using Trivo.Aplicacion.DTOs.Reportes;
using Trivo.Aplicacion.Interfaces.Repositorio.Cuenta;
using Trivo.Aplicacion.Paginacion;
using Trivo.Aplicacion.Utilidades;
using Trivo.Dominio.Modelos;

namespace Trivo.Aplicacion.Modulos.Administrador.Querys.ObtenerUsuariosBaneados;

internal sealed class ObtenerUsuariosBaneadosConPaginacionQueryHandler(
    ILogger<ObtenerUsuariosBaneadosConPaginacionQueryHandler> logger,
    IRepositorioAdministrador repositorioAdministrador,
    IDistributedCache cache
    ) : IQueryHandler<ObtenerUsuariosBaneadosConPaginacionQuery, ResultadoPaginado<ReporteBanDto>>
{
    public async Task<ResultadoT<ResultadoPaginado<ReporteBanDto>>> Handle(ObtenerUsuariosBaneadosConPaginacionQuery request, CancellationToken cancellationToken)
    {
        if (request.NumeroPagina <= 0 || request.TamanoPagina <= 0)
        {
            logger.LogWarning("");
            
            return ResultadoT<ResultadoPaginado<ReporteBanDto>>.Fallo(Error.Fallo("400",""));
        }

        // var resultadoPaginado = await cache.ObtenerOCrearAsync(
        //     $"obtener-ultimos-usuarios-baneados-{request.NumeroPagina}-{request.TamanoPagina}",
        //     async () =>
        //     {
        //         var pagina = await repositorioAdministrador.ObtenerPaginadoUltimosBan(request.NumeroPagina,
        //             request.TamanoPagina, cancellationToken);
        //
        //         var resultadoPaginaDto = pagina.Elementos!.Select(reporte => new ReporteBanDto
        //         (
        //             ReporteId: reporte.ReporteId ?? Guid.Empty,
        //             ReportadoPor: reporte.ReportadoPor,
        //             Reportado: new UsuarioReportado
        //             (
        //                 ReportadoId: reporte.,
        //                 Nombre: reporte.Reportado.Nombre
        //             )
        //         )).ToList();
        //
        //     }
        // );

        return null;
    }
}