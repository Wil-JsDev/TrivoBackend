using Trivo.Aplicacion.DTOs.Notificacion;

namespace Trivo.Aplicacion.Interfaces.Servicios.SignaIR;

public interface INotificadorDeNotificaciones
{
    Task NotificarNuevaNotificacion(Guid usuarioId, NotificacionDto notificacion);
    
    Task NotificarNotificacion(Guid usuarioId, IEnumerable<NotificacionDto> notificacion);
    
    Task NotificarNotificacionMarcadaComoLeida(Guid usuarioId,Guid notificacionId);
    
    Task NotificarNotificacionEliminada(Guid usuarioId,Guid notificacionId);
}