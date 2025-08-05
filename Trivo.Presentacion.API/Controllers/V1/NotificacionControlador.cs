using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Trivo.Aplicacion.DTOs.Notificacion;
using Trivo.Aplicacion.Interfaces.Servicios;

namespace Trivo.Presentacion.API.Controllers.V1;

[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/notifications")]
public class NotificacionControlador(
    INotificacionServicio notificacionServicio
    ) : ControllerBase
{

    [HttpPost]
    [Authorize]
    public async Task<IActionResult> CrearNotificacionMatchAsync([FromBody] CrearNotificacionDto parametroNotificacion,
        CancellationToken cancellationToken)
    {

        var resultado = await notificacionServicio.CrearNotificacionDeTipoAsync(parametroNotificacion.UsuarioId,
            parametroNotificacion.Tipo,
            parametroNotificacion.Contenido,
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