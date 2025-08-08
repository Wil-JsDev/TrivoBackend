using Trivo.Aplicacion.DTOs.Notificacion;

namespace Trivo.Aplicacion.Interfaces.Servicios.SignaIR;

public interface INotificacionHub
{
    Task RecibirNuevaNotificacion(NotificacionDto notificacion);
    
    Task RecibirNotificacion(IEnumerable<NotificacionDto> notificacion);
    
    Task NotificacionMarcadaComoLeida(Guid notificacionId, NotificacionDto notificacion);
    
    Task NotificarNotificacionEliminada(Guid notificacionId, NotificacionDto notificacion);
    
    // Task NotificarNotificacion(Guid usuarioId, NotificacionDto notificacion);
}