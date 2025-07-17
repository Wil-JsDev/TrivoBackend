using Trivo.Aplicacion.DTOs.Mensaje;

namespace Trivo.Infraestructura.Compartido.SignalR.Hubs;

public interface IChatHub
{
    Task RecibirMensajePrivado(MensajeDto mensaje);
    Task NotificacionNuevoMatch(string mensaje);
    Task NotificacionPendiente(string mensaje);
}