using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Trivo.Aplicacion.Modulos.Emparejamiento.Commands.Crear;
using Trivo.Aplicacion.Modulos.Emparejamiento.Commands.Rechazos;
using Trivo.Aplicacion.Modulos.Emparejamiento.Querys;
using Trivo.Dominio.Enum;

namespace Trivo.Presentacion.API.Controllers.V1;

[ApiVersion("1.0")]
[ApiController]
[Route("api/v{version:ApiVersion}/matches")]
public class EmparejamientoControlador(IMediator mediator) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> CrearEmparejamiento([FromBody] CrearEmparejamientoCommand emparejamientoCommand,
        CancellationToken cancellationToken)
    {
        var resultado = await mediator.Send(emparejamientoCommand, cancellationToken);
        if (!resultado.EsExitoso)
            return BadRequest(resultado.Error);
        
        return Ok(resultado.Valor);
    }

    [HttpPost("reject")]
    public async Task<IActionResult> RechazarEmparejamientoAsync([FromBody] CrearRechazosEmparejamientoCommand command,
        CancellationToken cancellationToken)
    {
        var resultado = await mediator.Send(command, cancellationToken);
        if (!resultado.EsExitoso)
            return BadRequest(resultado.Error);
        
        return Ok(resultado.Valor);
    }
    
    [HttpGet("filter")]
    public async Task<IActionResult> FiltrarEmparejamientosPorUsuarioAsync(
        [FromQuery] Guid usuarioId,
        [FromQuery] Roles roles,
        CancellationToken cancellationToken
    )
    {
        ObtenerEmparejamientoPorUsuarioQuery query = new(usuarioId, roles);
    
       var resultado= await mediator.Send(query, cancellationToken);
        if(resultado.EsExitoso)
            return Ok(resultado.Valor);
        
        return BadRequest(resultado.Error);
    }
    
}