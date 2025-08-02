using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Trivo.Aplicacion.Modulos.Mensajes.Commands.Crear;
using Trivo.Aplicacion.Modulos.Mensajes.Commands.EnviarArchivo;
using Trivo.Aplicacion.Modulos.Mensajes.Commands.EnviarImagen;
using Trivo.Aplicacion.Modulos.Mensajes.Querys;

namespace Trivo.Presentacion.API.Controllers.V1;

[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/messages")]
public class MensajesControlador(IMediator mediator) : ControllerBase
{
    
    [HttpPost]
    [Authorize]
    public async Task<IActionResult> EnviarMensaje([FromBody] EnviarMensajeCommand command, CancellationToken cancellationToken)
    {
        var resultado = await mediator.Send(command, cancellationToken);

        if (!resultado.EsExitoso)
        {
            return BadRequest(resultado.Error);
        }

        return Ok(resultado.Valor);
    }
    
    [HttpPost("image")]
    [Authorize]
    public async Task<IActionResult> EnviarImagen([FromForm] EnviarImagenCommand command, CancellationToken cancellationToken)
    {
        var resultado = await mediator.Send(command, cancellationToken);

        if (!resultado.EsExitoso)
        {
            return BadRequest(resultado.Error);
        }

        return Ok(resultado.Valor);
    }
    
    [HttpPost("file")]
    [Authorize]
    public async Task<IActionResult> EnviarArchivo([FromForm] EnviarArchivoCommand command, CancellationToken cancellationToken)
    {
        var resultado = await mediator.Send(command, cancellationToken);

        if (!resultado.EsExitoso)
        {
            return BadRequest(resultado.Error);
        }

        return Ok(resultado.Valor);
    }

}