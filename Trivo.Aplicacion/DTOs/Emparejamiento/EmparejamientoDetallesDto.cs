namespace Trivo.Aplicacion.DTOs.Emparejamiento;

public sealed record EmparejamientoDetallesDto
(
    Guid EmparejamientoId,
    Guid ReclutadotId,
    Guid ExpertoId,
    string ExpertoEstado,
    string ReclutadorEstado,
    string EmparejamientoEstado,
    DateTime? FechaRegistro
);