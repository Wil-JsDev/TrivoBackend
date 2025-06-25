using Trivo.Aplicacion.DTOs.Cuentas.Habilidades;
using Trivo.Aplicacion.DTOs.Cuentas.Intereses;

namespace Trivo.Aplicacion.DTOs.Cuentas.Usuarios;

public record UsuarioDetallesDto
(
    string? Nombre,
    string? Apellido,
    string? Ubicacion,
    string? Biografia,
    string? FotoPerfil,
    IEnumerable<HabilidadDto>? Habilidad,
    IEnumerable<InteresDto>? Interes
);