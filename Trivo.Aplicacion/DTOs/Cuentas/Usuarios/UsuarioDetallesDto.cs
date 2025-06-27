using Trivo.Aplicacion.DTOs.Habilidades;
using Trivo.Aplicacion.DTOs.Intereses;

namespace Trivo.Aplicacion.DTOs.Cuentas.Usuarios;

public record UsuarioDetallesDto
(
    string? Nombre,
    string? Apellido,
    string? Ubicacion,
    string? Biografia,
    string? FotoPerfil,
    IEnumerable<HabilidadNombreDto>? Habilidad,
    IEnumerable<InteresDto>? Interes
);