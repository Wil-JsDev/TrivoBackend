using Microsoft.AspNetCore.SignalR;
using Trivo.Aplicacion.DTOs.Notificacion;
using Trivo.Aplicacion.Interfaces.Servicios.SignaIR;
using Trivo.Infraestructura.Compartido.SignalR.Hubs;

namespace Trivo.Infraestructura.Compartido.SignalR;

public class NotificadorDeNotificaciones(
    IHubContext<NotificacionHub, INotificacionHub> hub
    ) : INotificadorDeNotificaciones
{
    public async Task NotificarNuevaNotificacion(Guid usuarioId, NotificacionDto notificacion)
    {
       await hub.Clients.User(usuarioId.ToString())
            .RecibirNuevaNotificacion(notificacion);
    }

    public async Task NotificarNotificacion(Guid usuarioId, IEnumerable<NotificacionDto> notificacion)
    {
        await hub.Clients.User(usuarioId.ToString())
            .RecibirNotificacion(notificacion);
    }

    public async Task NotificarNotificacionMarcadaComoLeida(Guid usuarioId, Guid notificacionId, NotificacionDto notificacion)
    {
        await hub.Clients.User(usuarioId.ToString())
            .NotificacionMarcadaComoLeida(notificacionId, notificacion);
    }
    
    public async Task NotificarNotificacionEliminada(Guid usuarioId, Guid notificacionId, NotificacionDto notificacion )
    {
        await hub.Clients.Users(usuarioId.ToString())
            .NotificarNotificacionEliminada(notificacionId, notificacion);
    }
}