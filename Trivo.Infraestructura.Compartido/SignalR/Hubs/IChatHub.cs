namespace Trivo.Infraestructura.Compartido.SignalR.Hubs;

public interface IChatHub
{
    Task RecibirMensajePrivado(Guid emisorId, string contenido);
    Task NotificacionNuevoMatch(string mensaje);
    Task NotificacionPendiente(string mensaje);
}