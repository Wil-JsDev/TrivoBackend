using Microsoft.AspNetCore.SignalR;
using Trivo.Aplicacion.DTOs.Chat;
using Trivo.Aplicacion.DTOs.Mensaje;
using Trivo.Aplicacion.Interfaces.Servicios.SignaIR;
using Trivo.Aplicacion.Paginacion;
using Trivo.Infraestructura.Compartido.SignalR.Hubs;

namespace Trivo.Infraestructura.Compartido.SignalR;

public class NotificadorTiempoReal(IHubContext<ChatHub, IChatHub> hub) : INotificadorTiempoReal
{
    
    public Task NotificarMensajePrivado(MensajeDto mensaje, Guid usuarioId)
        => hub.Clients.User(usuarioId.ToString())
            .RecibirMensajePrivado(mensaje);

    public Task NotificarMatchConfirmado(Guid usuarioId, string contenido)
        => hub.Clients.User(usuarioId.ToString())
            .NotificacionNuevoMatch(contenido);

    public Task NotificarMatchPendiente(Guid usuarioId, string contenido)
        => hub.Clients.User(usuarioId.ToString())
            .NotificacionPendiente(contenido);
    
    public async Task NotificarNuevoChat(Guid usuarioId, IEnumerable<ChatDto> chats)
    {
        foreach (var chat in chats)
        {
            await hub.Clients.User(usuarioId.ToString())
                .RecibirNuevoChat(chat);
        }
    }

    public async Task NotificarChatsPaginados(Guid usuarioId, ResultadoPaginado<ChatDto> resultado)
    {
        await hub.Clients.User(usuarioId.ToString())
            .RecibirChats(resultado);
    }

    public async Task NotificarPaginaMensajes(Guid usuarioId, Guid chatId, ResultadoPaginado<MensajeDto> pagina)
    {
        await hub.Clients.User(usuarioId.ToString())
            .RecibirMensajesDelChat(chatId, pagina);
    }
}
