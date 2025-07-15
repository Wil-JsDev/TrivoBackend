using Microsoft.AspNetCore.SignalR;
using Trivo.Aplicacion.Interfaces.Servicios.SignaIR;
using Trivo.Infraestructura.Compartido.SignalR.Hubs;

namespace Trivo.Infraestructura.Compartido.SignalR;

public class NotificadorTiempoReal(IHubContext<ChatHub, IChatHub> hub) : INotificadorTiempoReal
{
    
    public Task NotificarMensajePrivado(Guid receptorId, string contenido, Guid emisorId)
        => hub.Clients.User(receptorId.ToString())
            .RecibirMensajePrivado(emisorId, contenido);

    public Task NotificarMatchConfirmado(Guid usuarioId, string contenido)
        => hub.Clients.User(usuarioId.ToString())
            .NotificacionNuevoMatch(contenido);

    public Task NotificarMatchPendiente(Guid usuarioId, string contenido)
        => hub.Clients.User(usuarioId.ToString())
            .NotificacionPendiente(contenido);
}
