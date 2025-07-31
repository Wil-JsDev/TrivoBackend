using Trivo.Aplicacion.DTOs.Chat;
using Trivo.Aplicacion.DTOs.Mensaje;
using Trivo.Aplicacion.Paginacion;

namespace Trivo.Infraestructura.Compartido.SignalR.Hubs;

public interface IChatHub
{
    Task RecibirMensajePrivado(MensajeDto mensaje);
    Task RecibirChats(IEnumerable<ChatDto> chats);
    Task RecibirNuevoChat(ChatDto chat);
    Task RecibirMensajesDelChat(Guid chatId, IEnumerable<MensajeDto> mensajes);
    Task NotificacionNuevoMatch(string mensaje);
    Task NotificacionPendiente(string mensaje);
    
}