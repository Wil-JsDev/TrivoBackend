namespace Trivo.Aplicacion.DTOs.Usuario;

public record UsuarioChatDto(   
    Guid UsuarioId,
    string NombreUsuario,
    string NombreCompleto,
    string? FotoPerfil
    );