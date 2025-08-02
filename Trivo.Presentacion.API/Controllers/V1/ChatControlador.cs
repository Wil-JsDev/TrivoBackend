using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Trivo.Aplicacion.Modulos.Chat.Commands.Crear;

namespace Trivo.Presentacion.API.Controllers.V1;

[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/chats")]
public class ChatControlador(IMediator mediator) : ControllerBase
{             
    [HttpPost]
    public async Task<IActionResult> CrearChatAsync([FromBody] CrearChatCommand chatCommand,
        CancellationToken cancellationToken)
    {
        var resultado = await mediator.Send(chatCommand, cancellationToken);
        if (!resultado.EsExitoso)
        {
            return BadRequest(resultado.Error);
        }
        return Ok(resultado.Valor);
    }
}