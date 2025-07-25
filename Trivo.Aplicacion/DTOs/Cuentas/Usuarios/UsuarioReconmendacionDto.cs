using Trivo.Aplicacion.DTOs.Habilidades;
using Trivo.Aplicacion.DTOs.Intereses;

namespace Trivo.Aplicacion.DTOs.Cuentas.Usuarios;

public sealed record UsuarioReconmendacionDto
(
    Guid UsuarioId,
    string? Nombre,
    string? Apellido,
    string? Biografia,
    string? Posicion,
    List<InteresConIdDto> Intereses,
    List<HabilidadConIdDto> Habilidades,
    string? FotoPerfil
);