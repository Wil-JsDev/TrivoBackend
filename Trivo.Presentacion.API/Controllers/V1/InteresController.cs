using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Trivo.Aplicacion.Modulos.Intereses.Commands.Crear;
using Trivo.Aplicacion.Modulos.Intereses.Querys.ObtenerInteresCategoriaId;

namespace Trivo.Presentacion.API.Controllers.V1;

[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:ApiVersion}/interests")]
public class InteresController(IMediator mediator) : ControllerBase
{
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CrearInteresAsync([FromBody] CrearInteresCommand interesCommand, CancellationToken cancellationToken)
    {
        var resultado = await mediator.Send(interesCommand, cancellationToken);
        if (resultado.EsExitoso)
            return Ok(resultado);
        
        return BadRequest(resultado);
    }
    
    [HttpGet("by-categories")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> ObtenerPorCategoriasAsync(
        [FromQuery] List<Guid> categoriaIds,
        [FromQuery] int numeroPagina,
        [FromQuery] int tamanoPagina,
        CancellationToken cancellationToken)
    {
        ObtenerInteresesPorCategoriaQuery query = new(categoriaIds, numeroPagina, tamanoPagina);
        
        var resultado = await mediator.Send(query, cancellationToken);
        if (resultado.EsExitoso)
            return Ok(resultado);
        
        return BadRequest(resultado);
    }
}