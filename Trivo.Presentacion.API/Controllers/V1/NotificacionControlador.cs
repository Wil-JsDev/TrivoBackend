using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Trivo.Aplicacion.DTOs.Notificacion;
using Trivo.Aplicacion.Interfaces.Servicios;

namespace Trivo.Presentacion.API.Controllers.V1;

[ApiController]
[Route("api/v{version:apiVersion}/notifications")]
[ApiVersion("1.0")]
public class NotificacionControlador(
    INotificacionServicio notificacionServicio
    ) : ControllerBase
{

    [HttpPost("match")]
    [Authorize]
    public async Task<IActionResult> CrearNotificacionMatchAsync([FromBody] ParametroNotificacion parametroNotificacion,
        CancellationToken cancellationToken)
    {
        var resultado = await notificacionServicio.CrearNotificacionMatchAsync(parametroNotificacion.UsuarioId,
            parametroNotificacion.NombreUsuario!,
            cancellationToken);

        if (resultado.EsExitoso)
            return Ok(resultado.Valor);
        
        return BadRequest(resultado.Error);
    }
    
    [HttpPost("messages")]
    [Authorize]
    public async Task<IActionResult> CrearNotificacionMensajeAsync([FromBody] ParametroNotificacion parametroNotificacion,
        CancellationToken cancellationToken)
    {
        var resultado = await notificacionServicio.CrearNotificacionMensajeAsync(parametroNotificacion.UsuarioId,
            parametroNotificacion.NombreUsuario!,
            cancellationToken);

        if (resultado.EsExitoso)
            return Ok(resultado.Valor);
        
        return BadRequest(resultado.Error);
    }

    [HttpPut("{notificationId}/users/{userId}")]
    [Authorize]
    public async Task<IActionResult> MarcarNotificacionComoLeidaAsync(
        [FromRoute] Guid notificationId,
        [FromRoute]  Guid userId,
        CancellationToken cancellationToken)
    {
        var resultado = await notificacionServicio.MarcarComoLeidaAsync(notificationId,
            userId,
            cancellationToken);
        
        if (resultado.EsExitoso)
            return Ok(resultado.Valor);
        
        return BadRequest(resultado.Error);
    }
    
    
    
    
    
}