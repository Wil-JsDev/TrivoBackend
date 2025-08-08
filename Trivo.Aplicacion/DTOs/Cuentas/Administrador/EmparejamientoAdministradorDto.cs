namespace Trivo.Aplicacion.DTOs.Cuentas.Administrador;

public sealed record EmparejamientoAdministradorDto
(
    Guid EmparejamientoId,
    Guid ReclutadotId,
    Guid ExpertoId,
    string? ExpertoEstado,
    string? ReclutadorEstado,
    string? EmparejamientoEstado,
    DateTime? FechaRegistro,
    ReclutadorEmparejamientoDto? ReclutadorDto,
    ExpertoEmparejamientoDto? ExpertoDto
);

public sealed record ReclutadorEmparejamientoDto
(
    string? Nombre,
    string Apellido
);

public sealed record ExpertoEmparejamientoDto
(
    string? Nombre,
    string Apellido
);

