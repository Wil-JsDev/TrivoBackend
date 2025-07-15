using Microsoft.Extensions.Logging;
using Trivo.Aplicacion.Interfaces.Repositorio.Cuenta;
using Trivo.Aplicacion.Interfaces.Servicios;
using Trivo.Aplicacion.Utilidades;

namespace Trivo.Aplicacion.Servicios;

public class ValidarCorreo(
    ILogger<ValidarCorreo> logger,
    IRepositorioUsuario repositorioUsuario
    ) : IValidarCorreo
{
    public async Task<ResultadoT<bool>> ValidarCorreoAsync(string email, CancellationToken cancellationToken)
    {
        var existe = await repositorioUsuario.ExisteEmailAsync(email, cancellationToken);

        if (existe)
        {
            logger.LogInformation("El correo '{Correo}' ya está registrado.", email);
            return ResultadoT<bool>.Fallo(Error.Conflicto("409",$"Este {email} correo ya está en uso"));
        }

        logger.LogInformation("Este esta disponible para registrarse.");
        
        return ResultadoT<bool>.Exito(true);
    }
}