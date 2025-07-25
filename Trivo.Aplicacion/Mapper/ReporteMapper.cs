using Trivo.Aplicacion.DTOs.Reportes;
using Trivo.Dominio.Modelos;

namespace Trivo.Aplicacion.Mapper;
public static class ReporteMapper
{
    public static ReporteBanDto MapBanDto(Reporte reporte)
    {
        var mensaje = reporte.Mensaje!;
        Guid reportadoId;
        string? nombreReportado;

        if (mensaje.EmisorId == reporte.ReportadoPor)
        {
            reportadoId = mensaje.ReceptorId;
            nombreReportado = mensaje.Receptor?.Nombre;
        }
        else
        {
            reportadoId = mensaje.EmisorId ?? Guid.Empty;
            nombreReportado = mensaje.Emisor?.Nombre;
        }

        return new ReporteBanDto
        (
            ReporteId: reporte.ReporteId ?? Guid.Empty,
            ReportadoPor: reporte.ReportadoPor,
            Reportado: new UsuarioReportado
            (
                ReportadoId: reportadoId,
                Nombre: nombreReportado
            )
        );
    }
}
