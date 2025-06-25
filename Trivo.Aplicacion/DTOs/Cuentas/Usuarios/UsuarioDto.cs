using Trivo.Dominio.Enum;

namespace Trivo.Aplicacion.DTOs.Cuentas.Usuarios;

public sealed record UsuarioDto
(
    Guid? UsuarioId,
    string? Nombre, 
    string? Apellido, 
    string? Biografia,
    string? Email,
    string? NombreUsuario,
    string? Ubicacion,
    string? FotoPerfil,
    EstadoUsuario? EstadoUsuario,
    DateTime? FechaRegistro
);