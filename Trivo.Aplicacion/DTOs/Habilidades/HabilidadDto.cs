namespace Trivo.Aplicacion.DTOs.Habilidades;

public sealed record HabilidadDto
(
    Guid? HabilidadId,
    string Nombre,
    DateTime? FechaRegistro
);