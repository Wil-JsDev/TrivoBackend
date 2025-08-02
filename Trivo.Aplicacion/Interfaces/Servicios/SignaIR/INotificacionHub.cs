using Trivo.Aplicacion.DTOs.Notificacion;

namespace Trivo.Aplicacion.Interfaces.Servicios.SignaIR;

public interface INotificacionHub
{
    Task RecibirNuevaNotificacion(NotificacionDto notificacion);
    
    Task RecibirNotificacion(IEnumerable<NotificacionDto> notificacion);
    
    Task NotificacionMarcadaComoLeida(Guid notificacionId);
    
    // Task NotificarNotificacion(Guid usuarioId, NotificacionDto notificacion);
}