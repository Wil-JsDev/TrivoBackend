namespace Trivo.Aplicacion.DTOs.Reportes;

public sealed record MensajeDtoParaReporte
(
    Guid? MensajeId,
    Guid? EmisorId,
    string? Contenido,
    string? Tipo,
    DateTime? FechaEnvio,
    UsuarioDtoParaReporte? Emisor
);

public sealed record UsuarioDtoParaReporte
(
    Guid UsuarioId,
    string? Nombre,
    string? Apellido
);

public sealed record ReporteDto
(
    Guid ReporteId,
    Guid? ReportadoPor,
    Guid? MensajeId,
    string? Nota,
    string? EstadoReporte,
    MensajeDtoParaReporte? Mensaje, // Informacion del mensaje reportado con la informacion del usuario que lo reporto
    UsuarioDtoParaReporte? UsuarioReportadoPor, // Usuario que hizo el reporte
    UsuarioDtoParaReporte? UsuarioReportado     // Usuario que fue reportado
);