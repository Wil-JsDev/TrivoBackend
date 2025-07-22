using Trivo.Aplicacion.DTOs.Chat;
using Trivo.Aplicacion.DTOs.Mensaje;
using Trivo.Aplicacion.Paginacion;

namespace Trivo.Infraestructura.Compartido.SignalR.Hubs;

public interface IChatHub
{
    Task RecibirMensajePrivado(MensajeDto mensaje);
    Task RecibirChats(ResultadoPaginado<ChatDto> chats);
    Task RecibirNuevoChat(ChatDto chat);

    Task ObtenerChatsUsuario(int numeroPagina, int tamanoPagina);
    Task ObtenerMensajesChat(Guid chatId, int numeroPagina, int tamanoPagina);
    Task RecibirMensajesDelChat(Guid chatId, ResultadoPaginado<MensajeDto> mensajes);
    Task NotificacionNuevoMatch(string mensaje);
    Task NotificacionPendiente(string mensaje);
    Task NotificarNuevoChat(IEnumerable<ChatDto> chat);

}