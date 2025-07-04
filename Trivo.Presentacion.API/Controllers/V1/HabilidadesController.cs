using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Trivo.Aplicacion.Modulos.Habilidades.Commands.Crear;

namespace Trivo.Presentacion.API.Controllers.V1;

[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:ApiVersion}/ability")]
public class HabilidadesController(IMediator mediator) : ControllerBase
{
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CrearHabilidadesAsync([FromBody] CrearHabilidadCommand crearHabilidadCommand, CancellationToken cancellationToken)
    {
        var resultado = await mediator.Send(crearHabilidadCommand, cancellationToken);
        if (resultado.EsExitoso)
            return Ok(resultado.Valor);
        
        return BadRequest(resultado.Error);
    }
}