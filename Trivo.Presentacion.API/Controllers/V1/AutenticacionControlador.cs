using Microsoft.AspNetCore.Mvc;
using Trivo.Aplicacion.DTOs.JWT;
using Trivo.Aplicacion.Interfaces.Servicios;

namespace Trivo.Presentacion.API.Controllers.V1;

[ApiController]
[Route("api/v{version:apiVersion}/auth")]
public class AutenticacionControlador(
    IAutenticacionServicio autenticacionServicio
    ) : ControllerBase
{
    [HttpPost("refresh-token")]
    [ProducesResponseType(StatusCodes.Status200OK)] 
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> RefrescarTokenAsync( [FromBody] RefrescarTokenSolicitud request, CancellationToken cancellationToken)
    {
        var resultado = await autenticacionServicio.RefrescarTokenAsync(request.RefrescarToken!, cancellationToken);

        if (!resultado.EsExitoso)
            return Unauthorized(resultado.Error);

        return Ok(resultado.Valor);
    }
}