using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Trivo.Aplicacion.Modulos.Experto.Commands.Crear;

namespace Trivo.Presentacion.API.Controllers.V1;

[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:ApiVersion}/experts")]
public class ExpertoController(IMediator mediator) : ControllerBase
{
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CrearReclutadorAsync([FromBody] CrearExpertoCommand expertoCommand, CancellationToken cancellationToken)
    {
        var resultado = await mediator.Send(expertoCommand, cancellationToken);
        if (resultado.EsExitoso)
            return Ok(resultado.Valor);
        
        return BadRequest(resultado.Error);
    }
}