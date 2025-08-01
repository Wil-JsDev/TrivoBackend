using Microsoft.Extensions.Logging;
using Trivo.Aplicacion.DTOs.Notificacion;
using Trivo.Aplicacion.Interfaces.Repositorio;
using Trivo.Aplicacion.Interfaces.Repositorio.Cuenta;
using Trivo.Aplicacion.Interfaces.Servicios;
using Trivo.Aplicacion.Interfaces.Servicios.SignaIR;
using Trivo.Aplicacion.Paginacion;
using Trivo.Aplicacion.Utilidades;
using Trivo.Dominio.Modelos;

namespace Trivo.Aplicacion.Servicios;

public class NotificacionServicio(
    INotificadorDeNotificaciones notificador,
    ILogger<NotificacionServicio> logger,
    IRepositorioUsuario repositorioUsuario,
    IRepositorioNotificacion repositorioNotificacion
    ) : INotificacionServicio
{
    public async Task<ResultadoT<NotificacionDto>> CrearNotificacionAsync(CrearNotificacionDto notificacion, CancellationToken cancellationToken)
    {
        var usuario = await repositorioUsuario.ObtenerByIdAsync(notificacion.UsuarioId, cancellationToken);
        if (usuario is null)
        {
            logger.LogWarning("Usuario con ID {UsuarioId} no encontrado al intentar crear notificación", notificacion.UsuarioId);
            
            return ResultadoT<NotificacionDto>.Fallo(Error.NoEncontrado("404", "Usuario no encontrado"));
        }

        if (string.IsNullOrEmpty(notificacion.Contenido))
        {
            logger.LogWarning("Intento de crear notificación sin contenido para el usuario {UsuarioId}", notificacion.UsuarioId);
            
            return ResultadoT<NotificacionDto>.Fallo(Error.Fallo("400", "El contenido de la notificación no puede estar vacío"));
        }

        Notificacion notificacionEntidad = new()
        {
            NotificacionId = Guid.NewGuid(),
            UsuarioId = notificacion.UsuarioId,
            Tipo = notificacion.Tipo.ToString(),
            Contenido = notificacion.Contenido,
            Leida = false,
            FechaLeida = null
        };
        
        await repositorioNotificacion.CrearAsync(notificacionEntidad, cancellationToken);
        
        await repositorioNotificacion.CrearAsync(notificacionEntidad, cancellationToken);
        logger.LogInformation("Notificación creada exitosamente - ID: {NotificacionId}, Tipo: {Tipo}, Usuario: {UsuarioId}", 
            notificacionEntidad.NotificacionId, notificacionEntidad.Tipo, notificacionEntidad.UsuarioId);

        NotificacionDto notificacionDto = new
        (
            NotificacionId: notificacionEntidad.NotificacionId ?? Guid.Empty,
            UsuarioId: notificacionEntidad.UsuarioId ?? Guid.Empty,
            Tipo: notificacionEntidad.Tipo,
            Contenido: notificacionEntidad.Contenido,
            Leido: notificacionEntidad.Leida,
            FechaCreacion: notificacionEntidad.FechaCreacion,
            FechaLeido: notificacionEntidad.FechaLeida
        );
        
        await notificador.NotificarNuevaNotificacion(notificacion.UsuarioId, notificacionDto);

        logger.LogInformation("Notificación enviada en tiempo real al usuario {UsuarioId}", notificacion.UsuarioId);
        
        return ResultadoT<NotificacionDto>.Exito(notificacionDto);
    }

    public async Task<ResultadoT<ResultadoPaginado<NotificacionDto>>> ObtenerNotificacionesAsync(Guid usuarioId, int pagina, int tamanioPagina, CancellationToken cancellationToken)
    {
        
        if (pagina <= 0 || tamanioPagina <= 0)
        {
            logger.LogWarning("Parámetros de paginación inválidos. Página: {Pagina}, Tamaño página: {TamanioPagina}", 
                pagina, tamanioPagina);
        
            return ResultadoT<ResultadoPaginado<NotificacionDto>>.Fallo(
                Error.Fallo("400", "Los parámetros de paginación deben ser mayores a cero"));
        }
        
        var usuario = await repositorioUsuario.ObtenerByIdAsync(usuarioId, cancellationToken);
        if (usuario is null)
        {
            logger.LogWarning("Usuario con ID {UsuarioId} no encontrado al intentar obtener notificaciones", usuarioId);
            return ResultadoT<ResultadoPaginado<NotificacionDto>>.Fallo(
                Error.NoEncontrado("404", $"Usuario con ID {usuarioId} no encontrado"));
        }
        
        var notificaciones = await repositorioNotificacion.ObtenerNotificacionesPorUsuarioIdPaginadoAsync(usuarioId,
            pagina,
            tamanioPagina,
            cancellationToken);

        if (!notificaciones.Elementos!.Any())
        {
            logger.LogWarning("No se encontraron notificaciones para el usuario {UsuarioId}", usuarioId);
            
            return ResultadoT<ResultadoPaginado<NotificacionDto>>.Fallo(Error.Fallo("400","La lista esta vacia"));
        }
        
        var notificacionesDto = notificaciones.Elementos!.Select(n => new NotificacionDto
        (
            NotificacionId: n.NotificacionId ?? Guid.Empty,
            UsuarioId: n.UsuarioId ?? Guid.Empty,
            Tipo: n.Tipo,
            Contenido: n.Contenido,
            Leido: n.Leida,
            FechaCreacion: n.FechaCreacion,
            FechaLeido: n.FechaLeida
        )).ToList();
       
        logger.LogInformation("Se obtuvieron {Cantidad} notificaciones para el usuario {UsuarioId}", 
            notificacionesDto.Count, usuarioId);

        ResultadoPaginado<NotificacionDto> resultadoPaginado = new
        (
            elementos: notificacionesDto,
            totalElementos: notificaciones.TotalElementos,
            paginaActual: notificaciones.PaginaActual,
            tamanioPagina: tamanioPagina
        );
        
        await notificador.NotificarNotificacion(usuarioId, resultadoPaginado.Elementos!);
        
        logger.LogInformation("Notificacion enviada. Lista de notificaciones: {Notificaciones}", notificacionesDto.Count);
        
        return ResultadoT<ResultadoPaginado<NotificacionDto>>.Exito(resultadoPaginado);
    }

    public async Task<ResultadoT<NotificacionDto>> LeerNotificacionAsync(Guid notificacionId, CancellationToken cancellationToken)
    {
        if (notificacionId == Guid.Empty)
        {
            logger.LogWarning("Intento de marcar notificación con ID vacío");
            
            return ResultadoT<NotificacionDto>.Fallo(
                Error.Fallo("400", "El ID de notificación no puede estar vacío"));
        }

        var notificacionEntidad = await repositorioNotificacion.ObtenerByIdAsync(notificacionId, cancellationToken);
        if (notificacionEntidad is null)
        {
            logger.LogWarning("No se encontró la notificación con ID {NotificacionId} al intentar marcarla como leída", 
                notificacionId);
            return ResultadoT<NotificacionDto>.Fallo(
                Error.NoEncontrado("404", $"Notificación con ID {notificacionId} no encontrada"));
        }

        // Verificar si ya estaba marcada como leída
        var estabaLeidaPreviamente = notificacionEntidad.Leida ?? false;
    
        if (!estabaLeidaPreviamente)
        {
            notificacionEntidad.Leida = true;
            notificacionEntidad.FechaLeida = DateTime.UtcNow;
        
            await repositorioNotificacion.ActualizarAsync(notificacionEntidad, cancellationToken);
        }

        var notificacionDto = new NotificacionDto(
            NotificacionId: notificacionEntidad.NotificacionId ?? Guid.Empty,
            UsuarioId: notificacionEntidad.UsuarioId ?? Guid.Empty,
            Tipo: notificacionEntidad.Tipo,
            Contenido: notificacionEntidad.Contenido,
            Leido: notificacionEntidad.Leida,
            FechaCreacion: notificacionEntidad.FechaCreacion,
            FechaLeido: notificacionEntidad.FechaLeida
        );

        if (!estabaLeidaPreviamente)
        {
            await notificador.NotificarNotificacionMarcadaComoLeida(
                notificacionEntidad.UsuarioId ?? Guid.Empty,
                notificacionId);
            
            await notificador.NotificarNotificacion(
                notificacionEntidad.UsuarioId ?? Guid.Empty, 
                new List<NotificacionDto> { notificacionDto });
        }

        logger.LogInformation("Operación completada para notificación {NotificacionId}. Estado leído: {Leido}", 
            notificacionId, notificacionEntidad.Leida);
    
        return ResultadoT<NotificacionDto>.Exito(notificacionDto);
    }
}