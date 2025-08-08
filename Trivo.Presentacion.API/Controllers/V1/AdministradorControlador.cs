using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Trivo.Aplicacion.Modulos.Administrador.Commands.BanearUsuarios;
using Trivo.Aplicacion.Modulos.Administrador.Commands.CrearAdministrador;
using Trivo.Aplicacion.Modulos.Administrador.Commands.DesbanearUsuario;
using Trivo.Aplicacion.Modulos.Administrador.Commands.InicioSesion;
using Trivo.Aplicacion.Modulos.Administrador.Querys.ObtenerConteoDeEmparejamientos;
using Trivo.Aplicacion.Modulos.Administrador.Querys.ObtenerConteoDeUsuariosActivos;
using Trivo.Aplicacion.Modulos.Administrador.Querys.ObtenerConteoUsuariosReportados;
using Trivo.Aplicacion.Modulos.Administrador.Querys.ObtenerUltimosEmparejamientos;
using Trivo.Aplicacion.Modulos.Administrador.Querys.ObtenerUltimosUsuarios;
using Trivo.Aplicacion.Modulos.Administrador.Querys.ObtenerUsuariosBaneados;

namespace Trivo.Presentacion.API.Controllers.V1;

[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:ApiVersion}/admin")]
public class AdministradorControlador(IMediator mediator) : ControllerBase
{
   [HttpPost]
   public async Task<IActionResult> CrearAdministradorAsync([FromForm] CrearAdministradorCommand administradorCommand,
      CancellationToken cancellationToken)
   {
      var resultado = await mediator.Send(administradorCommand, cancellationToken);
      if (!resultado.EsExitoso)
         return BadRequest(resultado.Error);
      
      return Ok(resultado.Valor);
   }

   [HttpPost("auth")]
   public async Task<IActionResult> AutenticarAdministradorAsync([FromBody] InicioSesionAdminstradorCommand adminstradorCommand)
   {
      var resultado = await mediator.Send(adminstradorCommand);
      if (!resultado.EsExitoso)
         return BadRequest(resultado.Error);
      
      return Ok(resultado.Valor);
   }

   [HttpGet("count/match-complete")]
   [Authorize(Roles = "Administrador")]
   public async Task<IActionResult> ObtenerCantidadDeEmperajamientoCompletadosAsync(CancellationToken cancellationToken)
   {
      ObtenerConteoEmparejamientosCompletadosQuery query = new();
      var resultado = await mediator.Send(query, cancellationToken);
      if (!resultado.EsExitoso)
         return BadRequest(resultado.Error);
      
      return Ok(resultado.Valor);
   }

   [HttpGet("count/user-active")]
   [Authorize(Roles = "Administrador")]
   public async Task<IActionResult> ObtenerCantidadDeUsuariosActivosAsync(CancellationToken cancellationToken)
   {
      ObtenerConteoUsuariosActivosQuery query = new();
      var resultado = await mediator.Send(query, cancellationToken);
      if (!resultado.EsExitoso)
         return BadRequest(resultado.Error);
      
      return Ok(resultado.Valor);
   }

   [HttpGet("last-user")]
   [Authorize(Roles = "Administrador")]
   public async Task<IActionResult> ObtenerUltimoUsuarioRegistradoAsync(
      [FromQuery] int numeroPagina,
      [FromQuery] int tamanoPagina,
      CancellationToken cancellationToken)
   {
      ObtenerUltimosUsuariosPaginadosQuery query = new(numeroPagina, tamanoPagina);
      var resultado = await mediator.Send(query, cancellationToken);
      if (!resultado.EsExitoso)
         return BadRequest(resultado.Error);
      
      return Ok(resultado.Valor);
   }

   [HttpGet("banned-users")]
   [Authorize(Roles = "Administrador")]
   public async Task<IActionResult> ObtenerUltimos10BaneosDeUsuariosAsync(CancellationToken cancellationToken)
   {
      var resultado = await mediator.Send(new ObtenerUltimos10UsuariosBaneadosQuery(), cancellationToken);
      if (!resultado.EsExitoso)
         return BadRequest(resultado.Error);
      
      return Ok(resultado.Valor);
   }

   [HttpPut("users/{userId}/ban")]
   [Authorize(Roles = "Administrador")]
   public async Task<IActionResult> BanearUsuarioAsync([FromRoute] Guid userId, CancellationToken cancellationToken)
   {
      BanearUsuariosCommand banearUsuariosCommand = new(userId);
      var resultado = await mediator.Send(banearUsuariosCommand, cancellationToken);
      if (!resultado.EsExitoso)
         return BadRequest(resultado.Error);
      
      return Ok(resultado.Valor);
   }
   
   [HttpPut("users/{userId}/unban")]
   [Authorize(Roles = "Administrador")]
   public async Task<IActionResult> DesbanearUsuarioAsync([FromRoute] Guid userId, CancellationToken cancellationToken)
   {
      DesbanearUsuarioCommand command = new(userId);
      var resultado = await mediator.Send(command, cancellationToken);
      if (!resultado.EsExitoso)
         return BadRequest(resultado.Error);
      
      return Ok(resultado.Valor);
   }

   [HttpGet("last-match")]
   [Authorize(Roles = "Administrador")]
   public async Task<IActionResult> ObtenerUltimosEmparejamientosAsync(
      [FromQuery] int numeroPagina,
      [FromQuery] int tamanoPagina,
      CancellationToken cancellationToken
      )
   {
      ObtenerUltimosEmparejamientosQuery query = new(numeroPagina, tamanoPagina);
      var resultado = await mediator.Send(query, cancellationToken);
      if (resultado.EsExitoso)
         return Ok(resultado.Valor);

      return BadRequest(resultado.Error);
   }

   [HttpGet("count/users-report")]
   public async Task<IActionResult> ObtenerCantidadDeUsuariosReportadosAsync(CancellationToken cancellationToken)
   {
      var resultado = await mediator.Send(new ObtenerConteoDeUsuariosReportadosQuery(),cancellationToken);
      if (!resultado.EsExitoso)
         return BadRequest(resultado.Error);
      
      return Ok(resultado.Valor);
   }
}