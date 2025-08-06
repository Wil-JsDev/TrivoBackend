using Trivo.Aplicacion.DTOs.Notificacion;
using Trivo.Aplicacion.Paginacion;
using Trivo.Aplicacion.Utilidades;
using Trivo.Dominio.Enum;

namespace Trivo.Aplicacion.Interfaces.Servicios;

public interface INotificacionServicio
{
    Task<ResultadoT<ResultadoPaginado<NotificacionDto>>> ObtenerNotificacionesAsync(Guid usuarioId,
        int pagina,
        int tamanioPagina,
        CancellationToken cancellationToken);

    Task<ResultadoT<NotificacionDto>> MarcarComoLeidaAsync(Guid notificacionId, Guid usuarioId,
        CancellationToken cancellationToken);

    Task<ResultadoT<NotificacionDto>> EliminarNotificacionAsync(Guid notificacionId,
        CancellationToken cancellationToken);
    
    // Task<ResultadoT<NotificacionDto>> CrearNotificacionMatchAsync(
    //     Guid usuarioId,
    //     string nombreRemitente,
    //     CancellationToken cancellationToken);
    //
    // Task<ResultadoT<NotificacionDto>> CrearNotificacionMensajeAsync(
    //     Guid usuarioId,
    //     string nombreRemitente,
    //     CancellationToken cancellationToken);

    Task<ResultadoT<NotificacionDto>> CrearNotificacionDeTipoAsync(Guid usuarioId,
        string? tipoNotificacion,
        string? contenido,
        CancellationToken cancellationToken);
}