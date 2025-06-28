using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Trivo.Aplicacion.Modulos.Usuario.Commands.ConfirmarUsuario;
using Trivo.Aplicacion.Modulos.Usuario.Commands.Crear;

namespace Trivo.Presentacion.API.Controllers.V1;

[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:ApiVersion}/users")]
public class UsuarioControlador(IMediator mediator) : ControllerBase
{

    [HttpPost]
    [ProducesResponseType( StatusCodes.Status200OK)]
    [ProducesResponseType( StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> RegistrarUsuarioAsync([FromForm] CrearUsuarioCommand usuario, CancellationToken cancellationToken)
    {
        var resultado = await mediator.Send(usuario, cancellationToken);
        if (resultado.EsExitoso)
            return Ok(resultado);

        return BadRequest(resultado.Error);
    }
    
    [HttpPost("confirm-account")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> ConfirmarCuentaAsync(
        [FromQuery] Guid usuarioId,
        [FromQuery] string codigo,
        CancellationToken cancellationToken)
    {
        var confirmarUsuario = new ConfirmarUsuarioCommand(usuarioId, codigo);

        var resultado = await mediator.Send(confirmarUsuario, cancellationToken);
        if (resultado.EsExitoso)
            return Ok(resultado);

        return NotFound(resultado.Error);
    }
    
}