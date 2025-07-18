using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using Trivo.Aplicacion.DTOs.Mensaje;

namespace Trivo.Infraestructura.Compartido.SignalR.Hubs;

[Authorize]
public class ChatHub(
    ILogger<ChatHub> logger
    ): Hub<IChatHub>
{
    public override Task OnConnectedAsync()
    {
        logger.LogInformation($"Usuario conectado: {Context.UserIdentifier}");
        return base.OnConnectedAsync();
    }
    
    public override Task OnDisconnectedAsync(Exception? exception)
    {
        logger.LogInformation($"Usuario desconectado: {Context.UserIdentifier}");
        return base.OnDisconnectedAsync(exception);
    }
    
    public async Task EnviarMensaje(MensajeDto mensaje)
    {
        var emisorId = Context.UserIdentifier;
       
        
        if (!Guid.TryParse(emisorId, out var emisorGuid))
        {
            logger.LogWarning("UserIdentifier no es un GUID valido");
            return;
        }
        
        logger.LogInformation($" Usuario {emisorId} envia mensaje a {mensaje.ReceptorId}: {mensaje.Contenido}");

        await Clients.User(mensaje.ReceptorId.ToString())
            .RecibirMensajePrivado(mensaje with { EmisorId = emisorGuid });
    }
}