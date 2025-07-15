using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Trivo.Aplicacion.DTOs.Cuentas.Contrasenas;
using Trivo.Aplicacion.DTOs.Cuentas.Usuarios;
using Trivo.Aplicacion.DTOs.Habilidades;
using Trivo.Aplicacion.DTOs.Intereses;
using Trivo.Aplicacion.Modulos.Habilidades.Commands.Actualizar;
using Trivo.Aplicacion.Modulos.Intereses.Commands.Actualizar;
using Trivo.Aplicacion.Modulos.Usuario.Commands.Actualizar;
using Trivo.Aplicacion.Modulos.Usuario.Commands.ActualizarBiografia;
using Trivo.Aplicacion.Modulos.Usuario.Commands.ActualizarImagen;
using Trivo.Aplicacion.Modulos.Usuario.Commands.ConfirmarUsuario;
using Trivo.Aplicacion.Modulos.Usuario.Commands.Crear;
using Trivo.Aplicacion.Modulos.Usuario.Commands.InicioSesion;
using Trivo.Aplicacion.Modulos.Usuario.Commands.ModificarContrasena;
using Trivo.Aplicacion.Modulos.Usuario.Commands.OlvidarContrsena;
using Trivo.Aplicacion.Modulos.Usuario.Querys.ObtenerDetalles;
using Trivo.Aplicacion.Modulos.Usuario.Querys.ObtenerRecomendacionUsuarios;

namespace Trivo.Presentacion.API.Controllers.V1;

[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:ApiVersion}/users")]
public class UsuarioControlador(IMediator mediator) : ControllerBase
{

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> RegistrarUsuarioAsync([FromForm] CrearUsuarioCommand usuario,
        CancellationToken cancellationToken)
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
    public async Task<IActionResult> ObtenerDetallesPorIdUsuarioAsync([FromRoute] Guid usuarioId,
        CancellationToken cancellationToken)
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
        ModificarContrasenaUsuarioCommand command = new(usuarioId, parametroModificarContrasena.Codigo,
            parametroModificarContrasena.Contrasena, parametroModificarContrasena.ConfirmacionDeContrsena);
        var resultado = await mediator.Send(command, cancellationToken);
        if (resultado.EsExitoso)
            return Ok(resultado.Valor);

        return BadRequest(resultado.Error);
    }

    [HttpPut("{usuarioId}/info")]
    public async Task<IActionResult> ActualizarUsuarioAsync(
        [FromRoute] Guid usuarioId,
        [FromBody] ParametroActualizarUsuario parametroActualizarUsuario,
        CancellationToken cancellationToken
    )
    {
        ActualizarUsuarioCommand command = new(usuarioId, parametroActualizarUsuario.NombreUsuario,
            parametroActualizarUsuario.Email);
        var resultado = await mediator.Send(command, cancellationToken);
        if (resultado.EsExitoso)
            return Ok(resultado.Valor);

        return NotFound(resultado.Error);
    }

    [HttpPut("{usuarioId}/profile-photo")]
    public async Task<IActionResult> ActualizarFotoDePerfilAsync(
        [FromRoute] Guid usuarioId,
        [FromForm] ActualizarFotoDePerfilDto imagen,
        CancellationToken cancellationToken
    )
    {
        ActualizarImagenUsuarioCommand command = new(usuarioId, imagen.FotoPerfil);
        var resultado = await mediator.Send(command,cancellationToken); 
        if (resultado.EsExitoso)
            return Ok(resultado.Valor);
    
        return BadRequest(resultado.Error);
    }

    [HttpPut("{usuarioId}/biografia")]
    public async Task<IActionResult> ActualizarBiografiaAsync( 
        [FromRoute] Guid usuarioId,
        [FromBody] ParametroActualizarUsuarioBiografia actualizarUsuarioBiografia,
        CancellationToken cancellationToken
        )
    {
        ActualizarUsuarioBiografiaCommand command = new(usuarioId, actualizarUsuarioBiografia.Biografia);
        var resultado = await mediator.Send(command, cancellationToken);
        if (resultado.EsExitoso)
            return Ok(resultado.Valor);
        
        return NotFound(resultado.Error);
    }

    [HttpPut("{usuarioId}/intereses")]
    public async Task<IActionResult> ActualizarInteresesUsuario(
        [FromRoute] Guid usuarioId,
        [FromBody] ActualizarInteresDto dto,
        CancellationToken cancellationToken)
    {
        var comando = new ActualizarInteresCommand(usuarioId, dto.InteresIds);
        var resultado = await mediator.Send(comando, cancellationToken);

        if (resultado.EsExitoso)
            return Ok(resultado.Valor);

        return resultado.Error!.Codigo switch
        {
            "400" => BadRequest(resultado.Error),
            "404" => NotFound(resultado.Error)
        };

    }
    
    [HttpPut("{usuarioId}/habilidades")]
    public async Task<IActionResult> ActualizarHabilidadesUsuario(
        [FromRoute] Guid usuarioId,
        [FromBody] ActualizarHabilidadDto dto,
        CancellationToken cancellationToken)
    {
        var comando = new ActualizarHabilidadCommand(usuarioId, dto.HabilidadesIds);
        var resultado = await mediator.Send(comando, cancellationToken);

        if (resultado.EsExitoso)
            return Ok(resultado.Valor);

        return resultado.Error!.Codigo switch
        {
            "400" => BadRequest(resultado.Error),
            "404" => NotFound(resultado.Error)
        };
    }
    
    [HttpPost("recommended-users/stream")]
    public async Task<IActionResult> EmitirUsuariosRecomendadosTiempoReal([FromQuery] Guid usuarioId, CancellationToken cancellationToken)
    {
        RecomendacionUsuariosQuery query = new(usuarioId); 
        var resultado = await mediator.Send(query, cancellationToken);
        if (resultado.EsExitoso)
            return Ok(resultado.Valor);
        
        return BadRequest(resultado.Error);
    }
}
