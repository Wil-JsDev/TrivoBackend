using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Trivo.Aplicacion.DTOs.Intereses;
using Trivo.Aplicacion.Modulos.Intereses.Commands.Actualizar;
using Trivo.Aplicacion.Modulos.Intereses.Commands.Crear;
using Trivo.Aplicacion.Modulos.Intereses.Querys.BuscarPorNombre;
using Trivo.Aplicacion.Modulos.Intereses.Querys.ObtenerInteresCategoriaId;
using Trivo.Aplicacion.Modulos.Intereses.Querys.Paginacion;

namespace Trivo.Presentacion.API.Controllers.V1;

[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:ApiVersion}/interests")]
public class InteresControlador(IMediator mediator) : ControllerBase
{
    [HttpPost]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CrearInteresAsync([FromBody] CrearInteresCommand interesCommand, CancellationToken cancellationToken)
    {
        var resultado = await mediator.Send(interesCommand, cancellationToken);
        if (resultado.EsExitoso)
            return Ok(resultado.Valor);
        
        return BadRequest(resultado.Error);
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
            return Ok(resultado.Valor);
        
        return BadRequest(resultado.Error);
    }

    [HttpGet("pagination")]
    public async Task<IActionResult> ObtenerPaginacionDeInteresAsync(
        [FromQuery] int numeroPagina,
        [FromQuery] int tamanoPagina,
        CancellationToken cancellationToken
        )
    {
        PaginacionInteresQuery query = new(numeroPagina, tamanoPagina);
        var resultado = await mediator.Send(query, cancellationToken);
        if (resultado.EsExitoso)
            return Ok(resultado.Valor);
        
        return BadRequest(resultado.Error);
    }
    
    [HttpGet("search")]
    [Authorize]
    public async Task<IActionResult> BuscarInteresPorNombreAsync(
        [FromQuery] string nombre,
        CancellationToken cancellationToken
        )
    {
        BuscarInteresesPorNombreQuery query = new(nombre);
        var resultado = await mediator.Send(query, cancellationToken);
        if (resultado.EsExitoso)
            return Ok(resultado.Valor);

        return NotFound(resultado.Error);
    }
    
}