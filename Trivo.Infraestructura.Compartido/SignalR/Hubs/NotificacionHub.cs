using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using Trivo.Aplicacion.DTOs.Notificacion;
using Trivo.Aplicacion.Interfaces.Servicios;
using Trivo.Aplicacion.Interfaces.Servicios.SignaIR;

namespace Trivo.Infraestructura.Compartido.SignalR.Hubs;

[Authorize]
public class NotificacionHub(
    ILogger<NotificacionHub> logger,
    INotificacionServicio notificacionServicio
    ) : Hub<INotificacionHub>
{
    public override async Task OnConnectedAsync()
    {
        var userIdentifier = Context.UserIdentifier;

        var httpContext = Context.GetHttpContext();
        var query = httpContext?.Request.Query;

        var numeroPaginaString = query?["numeroPagina"];
        var tamanoPaginaString = query?["tamanoPagina"];
            
        var numeroPagina = int.TryParse(numeroPaginaString, out var np) ? np : 1;
        var tamanoPagina = int.TryParse(tamanoPaginaString, out var tp) ? tp : 5;
            
        logger.LogInformation("Usuario conectado:");
        logger.LogInformation("- UserIdentifier (SignalR): {UserIdentifier}", userIdentifier);

        if (!Guid.TryParse(userIdentifier, out var usuarioId))
        {
            logger.LogError("UserIdentifier no es un GUID válido");
            return;
        }

        logger.LogInformation("- UsuarioId: {UsuarioId}", usuarioId);
        
        var resultado = await notificacionServicio.ObtenerNotificacionesAsync(usuarioId, numeroPagina, tamanoPagina, CancellationToken.None);
        if (!resultado.EsExitoso)
        {
            logger.LogWarning("No se encontraron emparejamientos para el usuario {UsuarioId}", usuarioId);
            await Clients.User(usuarioId.ToString()).RecibirNotificacion(new List<NotificacionDto>());
            await base.OnConnectedAsync();
            return;
        }
        
        await Clients.User(usuarioId.ToString()).RecibirNotificacion(resultado.Valor.Elementos!);
        
        await base.OnConnectedAsync();
    }

    public async Task MarcarComoLeida(Guid notificacionId)
    {
        try
        {
            if ( !Guid.TryParse(Context.UserIdentifier, out var usuarioId) )
            {
                logger.LogWarning("Intento de marcar notificación con UserIdentifier inválido: {UserIdentifier}", Context.UserIdentifier);
                return;
            }

            logger.LogInformation("Solicitud para marcar notificación {NotificacionId} como leída por usuario {UsuarioId}", 
                notificacionId, usuarioId);

            var resultado = await notificacionServicio.MarcarComoLeidaAsync(notificacionId, usuarioId, CancellationToken.None);

            if ( !resultado.EsExitoso )
            {
                logger.LogWarning("Error al marcar notificación {NotificacionId}: {Error}", 
                    notificacionId, resultado.Error);
                
                return;
            }

            await Clients.User(usuarioId.ToString()).NotificacionMarcadaComoLeida(notificacionId);
            
            logger.LogInformation("Notificación {NotificacionId} marcada como leída exitosamente", notificacionId);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error inesperado al marcar notificación {NotificacionId} como leída", notificacionId);
            // await Clients.Caller.RecibirError(Error.Fallo("500", "Error interno del servidor"));
        }
    }
    
    public override Task OnDisconnectedAsync(Exception? exception)
    {
        logger.LogInformation("Usuario desconectado: {ContextUserIdentifier}", Context.UserIdentifier);
        
        return base.OnDisconnectedAsync(exception);
    }
    
}