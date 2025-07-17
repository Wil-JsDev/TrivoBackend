using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Trivo.Aplicacion.Modulos.Mensajes.Commands.Crear;
using Trivo.Aplicacion.Modulos.Mensajes.Querys;

namespace Trivo.Presentacion.API.Controllers.V1;

[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/mensajes")]
public class MensajesControlador(IMediator mediator) : ControllerBase
{
    
    [HttpPost]
    public async Task<IActionResult> EnviarMensaje([FromBody] EnviarMensajeCommand command, CancellationToken cancellationToken)
    {
        var resultado = await mediator.Send(command, cancellationToken);

        if (!resultado.EsExitoso)
        {
            return BadRequest(resultado.Error);
        }

        return Ok(resultado.Valor);
    }
    
    [HttpGet("{chatId}/pagination")]
    public async Task<IActionResult> ResultadoPaginadoChat(
        [FromRoute] Guid chatId,
        [FromQuery] int pageNumber,
        [FromQuery] int pageSize, CancellationToken cancellationToken)
    {
        ObtenerPaginasMensajesQuery query = new(chatId, pageNumber, pageSize);
        var resultado = await mediator.Send(query, cancellationToken);
        if (!resultado.EsExitoso)
            return BadRequest(resultado.Error);
        return Ok(resultado.Valor);
    }
}