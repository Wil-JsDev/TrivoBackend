using System.Security.Claims;
using Microsoft.AspNetCore.SignalR;
using Microsoft.IdentityModel.JsonWebTokens;

namespace Trivo.Infraestructura.Compartido.SignalR;

public class CustomUserIdProvider: IUserIdProvider
{
    public string? GetUserId(HubConnectionContext connection)
    {
        // var sub = connection.User?.FindFirst("sub")?.Value;
        //
        // // fallback por si el token viene con otro claim
        // if (string.IsNullOrWhiteSpace(sub))
        // {
        //     sub = connection.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        // }
        //
        // Console.WriteLine($"🧠 UserId (desde token): {sub}");
        //
        // return sub;
        var sub = connection.User?.FindFirst("sub")?.Value;
        if (string.IsNullOrWhiteSpace(sub))
            sub = connection.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        Console.WriteLine($"🧠 UserId (desde token): {sub}");
        return sub;
    }
}