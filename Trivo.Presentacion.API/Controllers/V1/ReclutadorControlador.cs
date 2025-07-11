using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Trivo.Aplicacion.DTOs.Reclutadores;
using Trivo.Aplicacion.Modulos.Reclutador.Commands.Actualizar;
using Trivo.Aplicacion.Modulos.Reclutador.Commands.Crear;

namespace Trivo.Presentacion.API.Controllers.V1;

[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:ApiVersion}/recruiters")]
public class ReclutadorControlador(IMediator mediator) : ControllerBase
{
    
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CrearReclutadorAsync([FromBody] CrearReclutadorCommand reclutadorCommand, CancellationToken cancellationToken)
    {
        var resultado = await mediator.Send(reclutadorCommand, cancellationToken);
        if (resultado.EsExitoso)
            return Ok(resultado.Valor);
        
        return BadRequest(resultado.Error);
    }

    [HttpPut("{reclutadorId}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> ActualizarReclutadorAsync(
        [FromRoute] Guid reclutadorId,
        [FromBody] ParametroActualizarReclutador parametroActualizarReclutador,
        CancellationToken cancellationToken
    )
    {
        ActualizarReclutadorCommand command = new(reclutadorId, parametroActualizarReclutador.NombreEmpresa);
        var resultado = await mediator.Send(command, cancellationToken);
        if (resultado.EsExitoso)
            return Ok(resultado.Valor);
        
        return NotFound(resultado.Error);
    }
    
}