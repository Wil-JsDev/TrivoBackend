using Trivo.Aplicacion.DTOs.Habilidades;
using Trivo.Aplicacion.DTOs.Intereses;

namespace Trivo.Aplicacion.DTOs.Cuentas.Usuarios;

public record UsuarioRecomendacionIaDto
(
    // Guid UsuarioId,
    string? Nombre,
    string? Apellido,
    string? Ubicacion,
    string? Biografia,
    string? Posicion,
    string? FotoPerfil,
    List<InteresConIdDto> Intereses,
    List<HabilidadConIdDto> Habilidades
);