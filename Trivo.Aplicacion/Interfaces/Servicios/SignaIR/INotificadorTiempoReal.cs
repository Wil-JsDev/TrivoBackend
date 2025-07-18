using Trivo.Aplicacion.DTOs.Mensaje;

namespace Trivo.Aplicacion.Interfaces.Servicios.SignaIR;

public interface INotificadorTiempoReal
{
    Task NotificarMensajePrivado(MensajeDto mensaje);
    Task NotificarMatchConfirmado(Guid usuarioId, string contenido);
    Task NotificarMatchPendiente(Guid usuarioId, string contenido);
}