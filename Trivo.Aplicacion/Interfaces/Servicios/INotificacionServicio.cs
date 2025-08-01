using Trivo.Aplicacion.DTOs.Notificacion;
using Trivo.Aplicacion.Paginacion;
using Trivo.Aplicacion.Utilidades;

namespace Trivo.Aplicacion.Interfaces.Servicios;

public interface INotificacionServicio
{
    Task<ResultadoT<NotificacionDto>> CrearNotificacionAsync(CrearNotificacionDto notificacion, CancellationToken cancellationToken);
    
    Task<ResultadoT<ResultadoPaginado<NotificacionDto>>> ObtenerNotificacionesAsync(Guid usuarioId,
        int pagina,
        int tamanioPagina,
        CancellationToken cancellationToken);
    
    Task<ResultadoT<NotificacionDto>> MarcarComoLeidaAsync(Guid notificacionId, CancellationToken cancellationToken);
}