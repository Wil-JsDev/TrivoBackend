using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using Trivo.Aplicacion.DTOs.Cuentas.Usuarios;
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
            return;
        }
        
        var httpContext = Context.GetHttpContext();
        var query = httpContext?.Request.Query;

        var numeroPaginaString = query?["numeroPagina"];
        var tamanoPaginaString = query?["tamanoPagina"];
        
        var numeroPagina = int.TryParse(numeroPaginaString, out var np) ? np : 1;
        var tamanoPagina = int.TryParse(tamanoPaginaString, out var tp) ? tp : 5;
        
        logger.LogInformation("- UsuarioId: {UsuarioId}", usuarioId);

        var resultado = await mediator.Send(new RecomendacionUsuariosQuery(
            usuarioId,
            NumeroPagina: numeroPagina,
            TamanoPagina: tamanoPagina
        ));

        if (!resultado.EsExitoso)
        {
            logger.LogWarning("No se encontraron recomendaciones para el usuario {UsuarioId}.", usuarioId);
            await Clients.User(usuarioId.ToString()).RecibirRecomendaciones(new List<UsuarioRecomendacionIaDto>());
            await base.OnConnectedAsync();
            return;
            
        }
        
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