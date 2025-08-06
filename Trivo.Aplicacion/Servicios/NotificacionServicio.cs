using Microsoft.Extensions.Logging;
using Trivo.Aplicacion.DTOs.Notificacion;
using Trivo.Aplicacion.Interfaces.Repositorio;
using Trivo.Aplicacion.Interfaces.Repositorio.Cuenta;
using Trivo.Aplicacion.Interfaces.Servicios;
using Trivo.Aplicacion.Interfaces.Servicios.SignaIR;
using Trivo.Aplicacion.Mapper;
using Trivo.Aplicacion.Paginacion;
using Trivo.Aplicacion.Utilidades;

namespace Trivo.Aplicacion.Servicios;

public class NotificacionServicio(
    INotificadorDeNotificaciones notificador,
    ILogger<NotificacionServicio> logger,
    IRepositorioUsuario repositorioUsuario,
    IRepositorioNotificacion repositorioNotificacion
    ) : INotificacionServicio
{
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

        var notificacionesDto = NotificacionMapper.MappearListaADto(notificaciones.Elementos!);
        
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

    public async Task<ResultadoT<NotificacionDto>> MarcarComoLeidaAsync(Guid notificacionId, Guid usuarioId ,CancellationToken cancellationToken)
    {
        if (notificacionId == Guid.Empty)
        {
            logger.LogWarning("Intento de marcar notificación con ID vacío");
            
            return ResultadoT<NotificacionDto>.Fallo(
                Error.Fallo("400", "El ID de notificación no puede estar vacío"));
        }

        var notificacionEntidad = await repositorioNotificacion.ObtenerPorIdYUsuarioAsync(
            notificacionId, 
            usuarioId,
            cancellationToken);
        
        if (notificacionEntidad is null)
        {
            logger.LogWarning("Notificación {NotificacionId} no encontrada para usuario {UsuarioId}", 
                notificacionId, usuarioId);
            
            return ResultadoT<NotificacionDto>.Fallo(
                Error.NoEncontrado("404", "Notificación no encontrada o no pertenece al usuario"));
        }

        
        if (notificacionEntidad.Leida.GetValueOrDefault())
        {
            logger.LogWarning("Notificación {NotificacionId} ya estaba marcada como leída", notificacionId);
            
            return ResultadoT<NotificacionDto>.Exito(
                NotificacionMapper.MapearNotificacionDto(notificacionEntidad));
        }
        
        notificacionEntidad.Leida = true;
        notificacionEntidad.FechaLeida = DateTime.UtcNow;

        await repositorioNotificacion.ActualizarAsync(notificacionEntidad, cancellationToken);

        var notificacionDto = NotificacionMapper.MapearNotificacionDto(notificacionEntidad);

        await notificador.NotificarNotificacionMarcadaComoLeida(usuarioId, notificacionId);
        await notificador.NotificarNotificacion(usuarioId, new List<NotificacionDto> { notificacionDto });

        logger.LogInformation("Notificación {NotificacionId} actualizada a estado 'leído' para usuario {UsuarioId}", 
            notificacionId, usuarioId);

        return ResultadoT<NotificacionDto>.Exito(notificacionDto);
    }
    
    public async Task<ResultadoT<NotificacionDto>> CrearNotificacionDeTipoAsync(Guid usuarioId,
        string? tipoNotificacion,
        string? contenido,
        CancellationToken cancellationToken)
    {
        var notificacionDto = new CrearNotificacionDto(
            UsuarioId: usuarioId,
            TipoNotificacion: tipoNotificacion,
            Contenido: contenido
        );
    
        if (usuarioId == Guid.Empty)
        {
            logger.LogWarning("Intento de crear notificación con UsuarioId vacío");
            
            return ResultadoT<NotificacionDto>.Fallo(Error.Fallo("400", "UsuarioId no puede estar vacío"));
        }

        if (string.IsNullOrEmpty(tipoNotificacion))
        {
            logger.LogInformation("El tipo de notificacion esta vacio");
            
            return ResultadoT<NotificacionDto>.Fallo(Error.Fallo("400","El tipo de notificacion no puede estar vacio"));
        }
        
        return await CrearNotificacionInternaAsync(notificacionDto, cancellationToken);
    }

    public async Task<ResultadoT<NotificacionDto>> EliminarNotificacionAsync(Guid notificacionId,
        Guid usuarioId,
        CancellationToken cancellationToken)
    {
        var notificacion = await repositorioNotificacion.ObtenerByIdAsync(notificacionId, cancellationToken);
        if (notificacion is null)
        {
            logger.LogInformation("No existe una notificacion con este id " + notificacionId);
            
            return ResultadoT<NotificacionDto>.Fallo(Error.NoEncontrado("404", "No existe una notificacion con este id " + notificacionId));
        }

        if (notificacionId == Guid.Empty)
        {
            logger.LogWarning("Intento de eliminar una notificacion fallido");
            
            return ResultadoT<NotificacionDto>.Fallo(Error.Fallo("400", "NotificacionId debe de ser un Guid valido"));
        }
        
        if (usuarioId == Guid.Empty)
        {
            logger.LogWarning("Intento de crear notificación con UsuarioId vacío");
            
            return ResultadoT<NotificacionDto>.Fallo(Error.Fallo("400", "UsuarioId no puede estar vacío"));
        }
        
        var usuario = await repositorioUsuario.ObtenerByIdAsync(usuarioId, cancellationToken);
        if (usuario is null)
        {
            logger.LogWarning("No existe un usuario con este Id {UsuarioId}", usuarioId);
            
            return ResultadoT<NotificacionDto>.Fallo(Error.NoEncontrado("404", $"No existe un usuario con este id {usuarioId}"));
        }
        
        await repositorioNotificacion.EliminarAsync(notificacion, cancellationToken);
        
        logger.LogInformation("Notificacion eliminada correctamente");
        
        var notificacionDto = NotificacionMapper.MapearNotificacionDto(notificacion);
        
        await notificador.NotificarNotificacionEliminada(usuarioId, notificacionId);
        await notificador.NotificarNotificacion(usuarioId, new List<NotificacionDto> { notificacionDto });

        logger.LogInformation("Notificacion eliminada correctamente en tiempo real");
        
        return ResultadoT<NotificacionDto>.Exito(NotificacionMapper.MapearNotificacionDto(notificacion));
    }
        
        
    #region Metodos Privados
    
        private async Task<ResultadoT<NotificacionDto>> CrearNotificacionInternaAsync(CrearNotificacionDto notificacion, CancellationToken cancellationToken)
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

            var notificacionEntidad = NotificacionMapper.MapearNotificacion(notificacion);
            
            await repositorioNotificacion.CrearAsync(notificacionEntidad, cancellationToken);
            
            logger.LogInformation("Notificación creada exitosamente - ID: {NotificacionId}, Tipo: {Tipo}, Usuario: {UsuarioId}", 
                notificacionEntidad.NotificacionId, notificacionEntidad.Tipo, notificacionEntidad.UsuarioId);

            var notificacionDto = NotificacionMapper.MapearNotificacionDto(notificacionEntidad);
            
            await NotificarYLoggear(notificacion.UsuarioId, notificacionDto);
            
            return ResultadoT<NotificacionDto>.Exito(notificacionDto);
        }
        
        
        private async Task NotificarYLoggear(Guid usuarioId, NotificacionDto notificacionDto)
        {
            try
            {
                await notificador.NotificarNuevaNotificacion(usuarioId, notificacionDto);
                logger.LogInformation("Notificación enviada en tiempo real - Usuario: {UsuarioId}, Notificación: {NotificacionId}", 
                    usuarioId, notificacionDto.NotificacionId);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error al notificar al usuario {UsuarioId}", usuarioId);
            }
        }
        
    #endregion

}