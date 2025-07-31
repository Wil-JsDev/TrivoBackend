using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Trivo.Aplicacion.Modulos.Administrador.Commands.CrearAdministrador;
using Trivo.Aplicacion.Modulos.Administrador.Commands.InicioSesion;
using Trivo.Aplicacion.Modulos.Administrador.Querys.ObtenerConteoDeEmparejamientos;
using Trivo.Aplicacion.Modulos.Administrador.Querys.ObtenerConteoDeUsuariosActivos;
using Trivo.Aplicacion.Modulos.Administrador.Querys.ObtenerUltimosUsuarios;

namespace Trivo.Presentacion.API.Controllers.V1;

[ApiVersion("1.0")]
[Route("api/v{version:ApiVersion}/admin")]
[ApiController]
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
   
   
}