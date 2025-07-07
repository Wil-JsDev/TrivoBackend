using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;

namespace Trivo.Infraestructura.Compartido.SignalR.Hubs;

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
    
    public async Task EnviarMensaje(Guid receptorId, string contenido)
    {
        var emisorId = Context.UserIdentifier;
        logger.LogInformation($"Usuario {emisorId} envia mensaje a {receptorId}: {contenido}");

        await Clients.User(receptorId.ToString())
            .RecibirMensajePrivado(Guid.Parse(emisorId), contenido);
    }
}