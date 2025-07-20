using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using Trivo.Aplicacion.DTOs.Mensaje;
using Trivo.Aplicacion.Modulos.Chat.Querys.Paginacion;
using Trivo.Aplicacion.Modulos.Mensajes.Querys;

namespace Trivo.Infraestructura.Compartido.SignalR.Hubs;

[Authorize]
public class ChatHub(
    ILogger<ChatHub> logger,
    IMediator mediator
    ): Hub<IChatHub>
{
    public override async Task OnConnectedAsync()
    {
        if (!Guid.TryParse(Context.UserIdentifier, out var userId))
        {
            logger.LogWarning("UserIdentifier no es un GUID válido: {UserIdentifier}", Context.UserIdentifier);
            return;
        }
        
        logger.LogInformation($"Usuario conectado: {userId}");
        
        await ObtenerChatsUsuario();
        await base.OnConnectedAsync();
    }
    
    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        var userId = Context.UserIdentifier;
        logger.LogInformation($"Usuario desconectado: {userId}");
        await base.OnDisconnectedAsync(exception);
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
    public async Task ObtenerChatsUsuario(int numeroPagina = 1, int tamanoPagina = 10)
    {
        if (!Guid.TryParse(Context.UserIdentifier, out var usuarioId))
            return;

        var chats = await mediator.Send(new ObtenerPaginasChatQuery(
            usuarioId,
            numeroPagina,
            tamanoPagina
        ));

        await Clients.Caller.RecibirChats(chats.Valor);
    }

    public async Task ObtenerMensajesChat(Guid chatId, int numeroPagina = 1, int tamanoPagina = 20)
    {
        var mensajes = await mediator.Send(new ObtenerPaginasMensajesQuery(
            chatId,
            numeroPagina,
            tamanoPagina
        ));

        await Clients.Caller.RecibirMensajesDelChat(chatId, mensajes.Valor);
    }
}