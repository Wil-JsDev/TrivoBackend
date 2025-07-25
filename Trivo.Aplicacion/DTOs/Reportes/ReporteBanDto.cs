namespace Trivo.Aplicacion.DTOs.Reportes;

public sealed record ReporteBanDto
(
    Guid ReporteId,
    Guid? ReportadoPor,
    UsuarioReportado Reportado
);
public sealed record UsuarioReportado(Guid? ReportadoId, string? Nombre);