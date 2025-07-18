using Microsoft.AspNetCore.SignalR;
using Trivo.Aplicacion.DTOs.Mensaje;
using Trivo.Aplicacion.Interfaces.Servicios.SignaIR;
using Trivo.Infraestructura.Compartido.SignalR.Hubs;

namespace Trivo.Infraestructura.Compartido.SignalR;

public class NotificadorTiempoReal(IHubContext<ChatHub, IChatHub> hub) : INotificadorTiempoReal
{
    
    public Task NotificarMensajePrivado(MensajeDto mensaje)
        => hub.Clients.User(mensaje.ReceptorId.ToString())
            .RecibirMensajePrivado(mensaje);

    public Task NotificarMatchConfirmado(Guid usuarioId, string contenido)
        => hub.Clients.User(usuarioId.ToString())
            .NotificacionNuevoMatch(contenido);

    public Task NotificarMatchPendiente(Guid usuarioId, string contenido)
        => hub.Clients.User(usuarioId.ToString())
            .NotificacionPendiente(contenido);
}
