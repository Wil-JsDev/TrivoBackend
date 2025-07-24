namespace Trivo.Aplicacion.DTOs.Cuentas.Administrador;

public sealed record AdministradorDto
(
    Guid? AdministradorId,
    string? Nombre, 
    string? Apellido, 
    string? Biografia,
    string? Email,
    string? NombreUsuario,
    string? FotoPerfil,
    DateTime? FechaRegistro
);