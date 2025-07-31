namespace Trivo.Aplicacion.DTOs.Usuario;

public sealed record UsuarioDto(
    Guid Id,
    string? Nombre,
    string? Apellido,
    string? FotoPerfil
    );