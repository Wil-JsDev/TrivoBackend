using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Trivo.Aplicacion.DTOs.Expertos;
using Trivo.Aplicacion.Modulos.Experto.Commands.Actualizar;
using Trivo.Aplicacion.Modulos.Experto.Commands.Crear;

namespace Trivo.Presentacion.API.Controllers.V1;

[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:ApiVersion}/experts")]
public class ExpertoControlador(IMediator mediator) : ControllerBase
{
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CrearExpertoAsync([FromBody] CrearExpertoCommand expertoCommand, CancellationToken cancellationToken)
    {
        var resultado = await mediator.Send(expertoCommand, cancellationToken);
        if (resultado.EsExitoso)
            return Ok(resultado.Valor);
        
        return BadRequest(resultado.Error);
    }

    [HttpPut("{expertoId}")]
    [Authorize(Roles = "Experto")]
    public async Task<IActionResult> ActualizarExpertoAsync(
        [FromRoute] Guid expertoId,
        [FromBody] ParametroActualizarExperto parametroActualizarExperto,
        CancellationToken cancellationToken
    )
    {
        ActualizarExpertoCommand command = new(expertoId,parametroActualizarExperto.DisponibleParaProyectos, parametroActualizarExperto.Contratado);
        var resultado = await mediator.Send(command, cancellationToken);
        if (resultado.EsExitoso)
            return Ok(resultado.Valor);
        
        return NotFound(resultado.Error);
    }
    
    
    
}