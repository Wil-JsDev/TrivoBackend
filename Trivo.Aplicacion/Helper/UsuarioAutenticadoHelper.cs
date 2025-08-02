using System.Security.Claims;
using Microsoft.AspNetCore.Http;

namespace Trivo.Aplicacion.Helper;

public static class UsuarioAutenticadoHelper
{
    public static Guid ObtenerUsuarioId(this HttpContext httpContext)
    {
        var claim = httpContext.User.FindFirst(ClaimTypes.NameIdentifier) ??   httpContext.User.FindFirst("sub");

        if (claim is null || !Guid.TryParse(claim.Value, out var usuarioId))
        {
            throw new UnauthorizedAccessException("Usuario no autenticado o claim inválido");
        }
        
        return usuarioId;
    }
}