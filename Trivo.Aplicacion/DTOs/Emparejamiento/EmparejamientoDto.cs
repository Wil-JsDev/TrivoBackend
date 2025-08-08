using Trivo.Aplicacion.DTOs.Cuentas.Expertos;
using Trivo.Aplicacion.DTOs.Cuentas.Reclutador;
using Trivo.Aplicacion.DTOs.Cuentas.Usuarios;

namespace Trivo.Aplicacion.DTOs.Emparejamiento;

public sealed record EmparejamientoDto
(
    Guid EmparejamientoId,
    Guid? ReclutadotId,
    Guid? ExpertoId,
    string? ExpertoEstado,
    string? ReclutadorEstado,
    string? EmparejamientoEstado,
    DateTime? FechaRegistro,
    ExpertoReconmendacionIaDto? ExpertoDto,
    ReclutadorReconmendacionIaDto? ReclutadorDto
);