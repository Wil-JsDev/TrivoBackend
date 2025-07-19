using System.IdentityModel.Tokens.Jwt;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using Trivo.Aplicacion.Interfaces.Servicios.SignaIR;
using Trivo.Aplicacion.Modulos.Usuario.Querys.ObtenerRecomendacionUsuarios;

namespace Trivo.Infraestructura.Compartido.SignalR.Hubs;

[Authorize]
public class RecomendacionUsuariosHub(
    ILogger<RecomendacionUsuariosHub> logger,
    IMediator mediator
    ) : Hub<IRecomendacionUsuariosHub>
{
    public override async Task OnConnectedAsync()
    {
        var userIdentifier = Context.UserIdentifier;

        logger.LogInformation("🔌 Usuario conectado:");
        logger.LogInformation("- UserIdentifier (SignalR): {UserIdentifier}", userIdentifier);

        if (!Guid.TryParse(userIdentifier, out var usuarioId))
        {
            logger.LogError("UserIdentifier no es un GUID válido");
            // Opcional: desconectar o devolver
            return;
        }
        
        logger.LogInformation("- UsuarioId: {UsuarioId}", usuarioId);

        var resultado = await mediator.Send(new RecomendacionUsuariosQuery(
            usuarioId,
            NumeroPagina: 1,
            TamanoPagina: 9
        ));

        await Clients.User(usuarioId.ToString())
            .RecibirRecomendaciones(resultado.Valor.Elementos);

        await base.OnConnectedAsync();
    }

    public override Task OnDisconnectedAsync(Exception? exception)
    {
        logger.LogInformation("Usuario desconectado: {ContextUserIdentifier}", Context.UserIdentifier);
        return base.OnDisconnectedAsync(exception);
    }

    // Si quieres permitir que el cliente llame métodos al servidor, se agregan aqui
}