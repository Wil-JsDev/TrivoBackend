using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Trivo.Aplicacion.DTOs.Cuentas.Contrasenas;
using Trivo.Aplicacion.DTOs.Cuentas.Usuarios;
using Trivo.Aplicacion.DTOs.Habilidades;
using Trivo.Aplicacion.DTOs.Intereses;
using Trivo.Aplicacion.Interfaces.Servicios;
using Trivo.Aplicacion.Modulos.Habilidades.Commands.Actualizar;
using Trivo.Aplicacion.Modulos.Intereses.Commands.Actualizar;
using Trivo.Aplicacion.Modulos.Usuario.Commands.Actualizar;
using Trivo.Aplicacion.Modulos.Usuario.Commands.ActualizarBiografia;
using Trivo.Aplicacion.Modulos.Usuario.Commands.ActualizarContrasenaAntigua;
using Trivo.Aplicacion.Modulos.Usuario.Commands.ActualizarImagen;
using Trivo.Aplicacion.Modulos.Usuario.Commands.ConfirmarUsuario;
using Trivo.Aplicacion.Modulos.Usuario.Commands.Crear;
using Trivo.Aplicacion.Modulos.Usuario.Commands.InicioSesion;
using Trivo.Aplicacion.Modulos.Usuario.Commands.ModificarContrasena;
using Trivo.Aplicacion.Modulos.Usuario.Commands.OlvidarContrsena;
using Trivo.Aplicacion.Modulos.Usuario.Querys.ObtenerBiografia;
using Trivo.Aplicacion.Modulos.Usuario.Querys.ObtenerDetalles;
using Trivo.Aplicacion.Modulos.Usuario.Querys.ObtenerFotoDePerfil;
using Trivo.Aplicacion.Modulos.Usuario.Querys.ObtenerHabilidades;
using Trivo.Aplicacion.Modulos.Usuario.Querys.ObtenerInteres;
using Trivo.Aplicacion.Modulos.Usuario.Querys.ObtenerRecomendacionUsuarios;
using Trivo.Aplicacion.Modulos.Usuario.Querys.ObtenerUsuariosPorHabilidadesInteres;

namespace Trivo.Presentacion.API.Controllers.V1;

[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:ApiVersion}/users")]
public class UsuarioControlador(
    IMediator mediator,
    IValidarCorreo validarCorreo,
    ICodigoServicio codigoServicio
    ) : ControllerBase
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
    [Authorize]
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

    [HttpPost("forgot-password")]
    public async Task<IActionResult> OlvidarContrasenaAsync([FromBody] OlvidarContrasenaUsuarioCommand olvidarContrasena,
        CancellationToken cancellationToken)
    {
        OlvidarContrasenaUsuarioCommand command = new(olvidarContrasena.Email);
        var resultado = await mediator.Send(command, cancellationToken);
        if (resultado.EsExitoso)
            return Ok(resultado.Valor);
    
        return NotFound(resultado.Error);
    }

    [HttpPost("modify-password")]
    public async Task<IActionResult> ModificarContrasenaAsync(
        [FromQuery] string email,
        [FromBody] ParametroModificarContrasenaDto parametroModificarContrasena,
        CancellationToken cancellationToken
    )
    {
        ModificarContrasenaUsuarioCommand command = new(email,
            parametroModificarContrasena.Contrasena,
            parametroModificarContrasena.ConfirmacionDeContrsena);
        
        var resultado = await mediator.Send(command, cancellationToken);
        if (resultado.EsExitoso)
            return Ok(resultado.Valor);

        return BadRequest(resultado.Error);
    }

    [HttpPut("{usuarioId}/info")]
    [Authorize]
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
    [Authorize]
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

    [HttpGet("{usuarioId}/profile-photo")]
    [Authorize]
    public async Task<IActionResult> ObtenerFotoDePerfilPorUsuarioIdAsync(
        [FromRoute] Guid  usuarioId,
        CancellationToken cancellationToken
        )
    {
        ObtenerFotoPerfilPorUsuarioIdQuery query = new(usuarioId);
        var resultado = await mediator.Send(query, cancellationToken);
        if (resultado.EsExitoso)
            return Ok(resultado.Valor);
        
        return NotFound(resultado.Error);
    }

    [HttpPut("{usuarioId}/bio")]
    [Authorize]
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

    [HttpGet("{usuarioId}/bio")]
    [Authorize]
    public async Task<IActionResult> ObtenerBiografiaPorUsuarioIdAsync([FromRoute] Guid usuarioId, CancellationToken cancellationToken)
    {
        ObtenerBiografiaPorUsuarioIdQuery query = new(usuarioId);
        var resultado = await mediator.Send(query, cancellationToken);
        if (resultado.EsExitoso)
            return Ok(resultado.Valor);
        
        return NotFound(resultado.Error);
    }
    
    [HttpPut("{usuarioId}/interests")]
    [Authorize]
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

    [HttpGet("{usuarioId}/interests")]
    [Authorize]
    public async Task<IActionResult> ObtenerBiogrfiaPorUsuarioIdAsync(
        [FromRoute] Guid usuarioId,
        CancellationToken cancellationToken
        )
    {
        ObtenerInteresPorUsuarioIdQuery query = new(usuarioId);
        var  resultado = await mediator.Send(query, cancellationToken);
        if (resultado.EsExitoso)
            return Ok(resultado.Valor);
        
        return NotFound(resultado.Error);
    }
    
    [HttpPut("{usuarioId}/ability")]
    [Authorize]
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

    [HttpGet("{usuarioId}/ability")]
    [Authorize]
    public async Task<IActionResult> ObtenerHabilidadesPorUsuarioIdAsync(
        [FromRoute]  Guid usuarioId,
        CancellationToken cancellationToken
        )
    {
        ObtenerHabilidadesPorUsuarioIdQuery query = new(usuarioId);
        var  resultado = await mediator.Send(query, cancellationToken);
        if (resultado.EsExitoso)
            return Ok(resultado.Valor);
        
        return NotFound(resultado.Error);
    }
    
    [HttpGet("verify-email")]
    public async Task<IActionResult> VerificarCorreo([FromQuery] string email, CancellationToken cancellationToken)
    {
        var resultado = await validarCorreo.ValidarCorreoAsync(email, cancellationToken);

        if (!resultado.EsExitoso)
            return BadRequest(resultado.Error);

        return Ok(resultado.Valor);
    }

    [HttpPost("filter-by-interests-and-ability")]
    [Authorize]
    public async Task<IActionResult> ObtenerUsuariosPorInteresesYHabilidades(
        [FromBody] ParametroUsuariFiltroHabilidadesInteres usuarisHabilidadesInteres,
        [FromQuery] int numeroPagina,
        [FromQuery] int tamanoPagina, 
        CancellationToken cancellationToken
        )
    {
        ObtenerUsuariosPorInteresesYHabilidadesQuery query = new(
            numeroPagina,
            tamanoPagina,
            usuarisHabilidadesInteres.HabilidadIds,
            usuarisHabilidadesInteres.InteresIds);
        
        var resultado = await mediator.Send(query, cancellationToken);
        if (resultado.EsExitoso)
            return Ok(resultado.Valor);
        
        return BadRequest(resultado.Error);
    }

    [HttpPost("validate-code/{code}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> ValidarCodigoAsync([FromRoute] string code, CancellationToken cancellationToken)
    {
        var resultado = await codigoServicio.ValidarCodigoAsync(code, cancellationToken);
        if (resultado.EsExitoso)
            return Ok(resultado.Valor);
        
        return BadRequest(resultado.Error);
    }
    
    [HttpPost("{userId}/change-password")]
    [Authorize]
    public async Task<IActionResult> CambiarContrasenaAntiguaAsync(
        [FromRoute] Guid userId,
        [FromBody] CambiarContrasenaAntiguaDto cambiarContrasenaAntiguaDto,
        CancellationToken cancellationToken
        )
    {
        ActualizarContrasenaAntiguaUsuarioCommand command = new(userId,
            cambiarContrasenaAntiguaDto.AntiguaContrasena,
            cambiarContrasenaAntiguaDto.NuevaContrasena,
            cambiarContrasenaAntiguaDto.ConfirmacionDeContrsena);
        
        var resultado = await mediator.Send(command, cancellationToken);
        if (resultado.EsExitoso)
            return Ok(resultado.Valor);
        
        return BadRequest(resultado.Error);
    }
    
}
