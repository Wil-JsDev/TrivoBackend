using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Trivo.Aplicacion.Modulos.Emparejamiento.Commands.Actualizar;
using Trivo.Aplicacion.Modulos.Emparejamiento.Commands.Crear;
using Trivo.Aplicacion.Modulos.Emparejamiento.Commands.Rechazos;

namespace Trivo.Presentacion.API.Controllers.V1;

[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:ApiVersion}/matches")]
public class EmparejamientoControlador(IMediator mediator) : ControllerBase
{
    [HttpPost]
    [Authorize]
    public async Task<IActionResult> CrearEmparejamiento([FromBody] CrearEmparejamientoCommand emparejamientoCommand,
        CancellationToken cancellationToken)
    {
        var resultado = await mediator.Send(emparejamientoCommand, cancellationToken);
        if (!resultado.EsExitoso)
            return BadRequest(resultado.Error);
        
        return Ok(resultado.Valor);
    }

    [HttpPut]
    [Authorize]
    public async Task<IActionResult> ActualizarEstadoEmparejamientoAsync(
        [FromBody] ActualizarEmparejamientoCommand command,
        CancellationToken cancellationToken)
    {
        var resultado = await mediator.Send(command, cancellationToken);
        if (!resultado.EsExitoso)
            return BadRequest(resultado.Error);
        
        return Ok(resultado.Valor);
    }
    
    [HttpPost("reject")]
    [Authorize]
    public async Task<IActionResult> RechazarEmparejamientoAsync([FromBody] CrearRechazosEmparejamientoCommand command,
        CancellationToken cancellationToken)
    {
        var resultado = await mediator.Send(command, cancellationToken);
        if (!resultado.EsExitoso)
            return BadRequest(resultado.Error);
        
        return Ok(resultado.Valor);
    }
}