using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Trivo.Aplicacion.Modulos.Administrador.Commands.CrearAdministrador;
using Trivo.Aplicacion.Modulos.Administrador.Commands.InicioSesion;

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
}