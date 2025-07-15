namespace Trivo.Aplicacion.Interfaces.Servicios;

public interface INotificadorTiempoReal
{
    Task NotificarMensajePrivado(Guid receptorId, string contenido, Guid emisorId);
    Task NotificarMatchConfirmado(Guid usuarioId, string contenido);
    Task NotificarMatchPendiente(Guid usuarioId, string contenido);
}