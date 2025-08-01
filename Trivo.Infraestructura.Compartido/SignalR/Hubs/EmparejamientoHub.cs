using System.Security.Claims;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using Trivo.Aplicacion.DTOs.Emparejamiento;
using Trivo.Aplicacion.Interfaces.Servicios.SignaIR;
using Trivo.Aplicacion.Modulos.Emparejamiento.Querys;
using Trivo.Dominio.Enum;

namespace Trivo.Infraestructura.Compartido.SignalR.Hubs;

[Authorize]
public class EmparejamientoHub(
    ILogger<EmparejamientoHub> logger,
    IMediator mediator
    ) : Hub<IEmparejamientoHub>
{
  public override async Task OnConnectedAsync()
    {
        try
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

            var claims = Context.User?.Claims.ToList();

            if (claims == null || !claims.Any())
            {
                logger.LogWarning("No se encontraron claims en el usuario.");
                return;
            }

            foreach (var claim in claims)
            {
                logger.LogInformation("Claim: {Type} = {Value}", claim.Type, claim.Value);
            }

            var rolesClaims = claims
                .Where(c => c.Type == ClaimTypes.Role)
                .Select(c => c.Value)
                .ToList();

            if (rolesClaims.Count == 0)
            {
                logger.LogWarning("No se encontraron claims de roles.");
                return;
            }

            Roles rol = default;
            if (!rolesClaims.Any(rc => Enum.TryParse(rc, ignoreCase: true, out rol)))
            {
                logger.LogWarning("Ninguno de los roles en el token es válido. Roles encontrados: {Roles}", string.Join(", ", rolesClaims));
                return;
            }

            logger.LogInformation("Usuario con rol válido conectado: {UsuarioId} - Rol: {Rol}", usuarioId, rol.ToString());

            var resultado = await mediator.Send(new ObtenerEmparejamientoPorUsuarioQuery
            (
                usuarioId,
                numeroPagina,
                tamanoPagina,
                rol
            ));

            if (!resultado.EsExitoso)
            {
                logger.LogWarning("No se encontraron emparejamientos para el usuario {UsuarioId} con el rol {Rol}.", usuarioId, rol);
                await Clients.User(usuarioId.ToString()).RecibirEmparejamiento(new List<EmparejamientoDto>());
                await base.OnConnectedAsync();
                return;
            }

            await Clients.User(usuarioId.ToString()).RecibirEmparejamiento(resultado.Valor);

            await base.OnConnectedAsync();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error en OnConnectedAsync para el usuario SignalR.");
            throw; 
        }
    }

    public override Task OnDisconnectedAsync(Exception? exception)
    {
        logger.LogInformation("Usuario desconectado: {ContextUserIdentifier}", Context.UserIdentifier);
        
        return base.OnDisconnectedAsync(exception);
    }
}