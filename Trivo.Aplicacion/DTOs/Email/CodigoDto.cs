namespace Trivo.Aplicacion.DTOs.Email;

public sealed record CodigoDto
(
    Guid CodigoId,
    Guid UsuarioId,
    string Codigo,
    bool Usado,
    DateTime? Expiracion
);