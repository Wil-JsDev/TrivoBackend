using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using Trivo.Aplicacion.Interfaces.Servicios.SignaIR;

namespace Trivo.Infraestructura.Compartido.SignalR.Hubs;

[Authorize]
public class NotificacionHub(
    ILogger<NotificacionHub> logger
    ) : Hub<INotificacionHub>
{
    public override async Task OnConnectedAsync()
    {
        
        
        await base.OnConnectedAsync();
    }
 
    public override Task OnDisconnectedAsync(Exception? exception)
    {
        logger.LogInformation("Usuario desconectado: {ContextUserIdentifier}", Context.UserIdentifier);
        
        return base.OnDisconnectedAsync(exception);
    }
    
}