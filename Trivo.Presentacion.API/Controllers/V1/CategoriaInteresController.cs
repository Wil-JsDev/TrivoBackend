using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Trivo.Aplicacion.Modulos.CategoriaIntereses.Commands;
using Trivo.Aplicacion.Modulos.CategoriaIntereses.Querys.Paginacion;

namespace Trivo.Presentacion.API.Controllers.V1;

[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:ApiVersion}/category-interests")]
public class CategoriaInteresControlador(IMediator mediator) : ControllerBase
{
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CrearCategoriaInteres([FromBody] CrearCategoriaInteresCommand categoriaInteresCommand, CancellationToken cancellationToken)
    {
        var resultado = await mediator.Send(categoriaInteresCommand, cancellationToken);
        if (resultado.EsExitoso)
            return Ok(resultado);
        
        return BadRequest(resultado);
    }

    [HttpGet("pagination")]
    public async Task<IActionResult> ObtenerPaginacion(
        [FromQuery] int numeroPagina,
        [FromQuery] int tamanoPagina,
        CancellationToken cancellationToken
        )
    {
        ObtenerCategoriaInteresPaginadoQuery query = new(numeroPagina, tamanoPagina);
        var resultado = await mediator.Send(query, cancellationToken);
        if (resultado.EsExitoso)
            return Ok(resultado);
        
        return BadRequest(resultado);
    }
    
}