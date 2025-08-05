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
    
    // public static ReporteDto MapToReporteDto(Reporte reporte)
    // {
    //     if (reporte == null) return null;
    //
    //     // Ejemplo: Si el reporte es sobre el emisor del mensaje (quien lo envió)
    //     bool incluirEmisorEnMensajeDto = true; // Cambia a `false` si necesitas al receptor
    //
    //     return new ReporteDto(
    //         reporte.ReporteId ?? Guid.Empty,
    //         reporte.ReportadoPor,
    //         reporte.MensajeId,
    //         reporte.Nota,
    //         reporte.EstadoReporte,
    //         reporte.Mensaje != null 
    //             ? MensajeDtoParaReporte(reporte.Mensaje, incluirEmisorEnMensajeDto) 
    //             : null,
    //         reporte.UsuarioReportadoPor != null 
    //             ? new UsuarioDtoParaReporte(
    //                 reporte.UsuarioReportadoPor.UsuarioId,
    //                 reporte.UsuarioReportadoPor.Nombre,
    //                 reporte.UsuarioReportadoPor.Apellido
    //             ) 
    //             : null
    //     );
    // }
    //
    
}
