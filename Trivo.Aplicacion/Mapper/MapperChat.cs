using Trivo.Aplicacion.DTOs.Chat;
using Trivo.Aplicacion.DTOs.Mensaje;
using Trivo.Aplicacion.DTOs.Usuario;

namespace Trivo.Aplicacion.Mapper;

public class MapperChat
{
    public static ChatDto MapChatToDto(Dominio.Modelos.Chat chat, Guid usuarioConsultanteId)
    {
        var usuariosDtos = chat.ChatUsuarios.Select(cu => new UsuarioChatDto(
            cu.UsuarioId!.Value,
            cu.Usuario?.NombreUsuario ?? "",
            $"{cu.Usuario?.Nombre} {cu.Usuario?.Apellido}".Trim(),
            cu.Usuario?.FotoPerfil
        )).ToList();
        
        var nombreChat = chat.ChatUsuarios
            .Where(cu => cu.UsuarioId != usuarioConsultanteId)  
            .Select(cu => $"{cu.Usuario?.Nombre} {cu.Usuario?.Apellido}".Trim())
            .FirstOrDefault() ?? "Sin nombre";  

        var ultimoMensaje = chat.Mensajes?
            .OrderByDescending(m => m.FechaEnvio)
            .FirstOrDefault();

        MensajeDto? ultimoMensajeDto = null;

        if (ultimoMensaje is not null)
        {
            var emisorId = ultimoMensaje.EmisorId ?? Guid.Empty;

            ultimoMensajeDto = new MensajeDto(
                ultimoMensaje.MensajeId!.Value,
                chat.Id!.Value,
                emisorId,
                ultimoMensaje.Estado ?? string.Empty,
                ultimoMensaje.Contenido ?? string.Empty,
                ultimoMensaje.FechaEnvio ?? DateTime.UtcNow
            );
        }

        return new ChatDto(
            Id: chat.Id ?? Guid.Empty,
            Participantes: usuariosDtos,
            Nombre: nombreChat,  
            FechaCreacion: chat.FechaRegistro ?? DateTime.UtcNow,
            UltimoMensaje: ultimoMensajeDto
        );
    }
}