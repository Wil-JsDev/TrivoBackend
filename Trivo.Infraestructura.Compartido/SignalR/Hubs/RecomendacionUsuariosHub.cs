using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using Trivo.Aplicacion.Interfaces.Servicios.SignaIR;

namespace Trivo.Infraestructura.Compartido.SignalR.Hubs;

[Authorize]
public class RecomendacionUsuariosHub(
    ILogger<RecomendacionUsuariosHub> logger
    ) : Hub<IRecomendacionUsuariosHub>
{
    public override Task OnConnectedAsync()
    {
        var userIdentifier = Context.UserIdentifier;
        var isAuth = Context.User?.Identity?.IsAuthenticated ?? false;
        var sub = Context.User?.FindFirst(JwtRegisteredClaimNames.Sub)?.Value;

        logger.LogInformation("🔌 Usuario conectado:");
        logger.LogInformation("- UserIdentifier (SignalR): {UserIdentifier}", userIdentifier);
        logger.LogInformation("- sub desde Claims: {Sub}", sub);
        logger.LogInformation("- ¿Autenticado?: {IsAuth}", isAuth);
        
        return base.OnConnectedAsync();
    }

    public override Task OnDisconnectedAsync(Exception? exception)
    {
        logger.LogInformation("Usuario desconectado: {ContextUserIdentifier}", Context.UserIdentifier);
        return base.OnDisconnectedAsync(exception);
    }

    // Si quieres permitir que el cliente llame métodos al servidor, se agregan aqui
}