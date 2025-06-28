namespace Trivo.Aplicacion.DTOs.Intereses;

public sealed record InteresDetallesDto
(
    Guid InteresId,
    string Nombre,
    Guid? CategoriaId,
    Guid CreadoPor
);