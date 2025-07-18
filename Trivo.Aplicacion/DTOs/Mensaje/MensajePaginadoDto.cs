using Trivo.Aplicacion.DTOs.Usuario;

namespace Trivo.Aplicacion.DTOs.Mensaje;

public record MensajePaginadoDto(Guid Id,
    Guid ChatId,
    string Contenido,
    string Estado,
    DateTime FechaEnvio,
    UsuarioChatDto Emisor 
    );