using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Trivo.Aplicacion.Modulos.Habilidades.Commands.Crear;
using Trivo.Aplicacion.Modulos.Habilidades.Querys.Paginacion;

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

    [HttpGet("pagination")]
    public async Task<IActionResult> ObtenerPaginacionHabilidadesAsync(
        [FromQuery] int numeroPagina,
        [FromQuery] int tamanoPagina,
        CancellationToken cancellationToken
    )
    {
        ObtenerPaginasHabilidadesQuery query = new(numeroPagina, tamanoPagina);
        var resultado = await mediator.Send(query, cancellationToken);
        if (resultado.EsExitoso)
            return Ok(resultado.Valor);
        
        return BadRequest(resultado.Error);
    }
    
}