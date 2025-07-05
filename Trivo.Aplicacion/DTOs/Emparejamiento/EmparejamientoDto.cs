namespace Trivo.Aplicacion.DTOs.Emparejamiento;

public sealed record EmparejamientoDto
(
    Guid EmparejamientoId,
    Guid ReclutadorId,
    Guid ExpertoId,
    string EstadoExperto,
    string EstadoReclutador,
    string EstadoEmparejamiento
);