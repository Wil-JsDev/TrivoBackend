using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Trivo.Aplicacion.Modulos.Reportes.Commands.Crear;

namespace Trivo.Presentacion.API.Controllers.V1;

[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:ApiVersion}/report")]
public class ReporteControlador(IMediator mediator) : ControllerBase
{
    [HttpPost]
    [Authorize]
    public async Task<IActionResult> CrearReporteAsync([FromBody] CrearReporteCommand request, CancellationToken cancellationToken)
    {
        var resultado = await mediator.Send(request, cancellationToken);
        if(resultado.EsExitoso)
            return Ok(resultado.Valor);
        
        return NotFound(resultado.Error);
    }
}