using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Trivo.Aplicacion.DTOs.Cuentas.Contrasenas;
using Trivo.Aplicacion.Modulos.Usuario.Commands.ConfirmarUsuario;
using Trivo.Aplicacion.Modulos.Usuario.Commands.Crear;
using Trivo.Aplicacion.Modulos.Usuario.Commands.InicioSesion;
using Trivo.Aplicacion.Modulos.Usuario.Commands.ModificarContrasena;
using Trivo.Aplicacion.Modulos.Usuario.Commands.OlvidarContrsena;
using Trivo.Aplicacion.Modulos.Usuario.Querys.ObtenerDetalles;

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
            return Ok(resultado.Valor);

        return NotFound(resultado.Error);
    }

    [HttpGet("profile/{usuarioId}")]
    public async Task<IActionResult> ObtenerDetallesPorIdUsuarioAsync([FromRoute] Guid usuarioId, CancellationToken cancellationToken)
    {
        ObtenerDetallesUsuarioQuery query = new(usuarioId);
        var resultado = await mediator.Send(query, cancellationToken);
        if (resultado.EsExitoso)
            return Ok(resultado.Valor);
        
        return BadRequest(resultado.Error);
    }
    
    [HttpPost("auth")]
    public async Task<IActionResult> InicioSesionAsync([FromBody] InicioSesionUsuarioCommand command,
        CancellationToken cancellationToken)
    {
        var resultado = await mediator.Send(command, cancellationToken);
        if (resultado.EsExitoso)
            return Ok(resultado.Valor);
        
        return BadRequest(resultado.Error);
    }

    [HttpPost("forgot-password/{usuarioId}")]
    public async Task<IActionResult> OlvidarContrasenaAsync([FromRoute] Guid usuarioId,
        CancellationToken cancellationToken)
    {
        OlvidarContrasenaUsuarioCommand command = new(usuarioId);
        var resultado = await mediator.Send(command, cancellationToken);
        if (resultado.EsExitoso)
            return Ok(resultado.Valor);
        
        return NotFound(resultado.Error);
    }

    [HttpPost("modify-password/{usuarioId}")]
    public async Task<IActionResult> ModificarContrasenaAsync(
        [FromRoute] Guid usuarioId,
        [FromBody] ParametroModificarContrasenaDto parametroModificarContrasena,
        CancellationToken cancellationToken
        )
    {
        ModificarContrasenaUsuarioCommand command = new(usuarioId, parametroModificarContrasena.Codigo, parametroModificarContrasena.Contrasena, parametroModificarContrasena.ConfirmacionDeContrsena);
        var resultado = await mediator.Send(command, cancellationToken);
        if (resultado.EsExitoso)
            return Ok(resultado.Valor);
        
        return BadRequest(resultado.Error);
    }
    
}
