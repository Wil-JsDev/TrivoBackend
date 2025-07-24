using Microsoft.Extensions.Logging;
using Trivo.Aplicacion.Abstracciones.Mensajes;
using Trivo.Aplicacion.DTOs.JWT;
using Trivo.Aplicacion.Interfaces.Repositorio.Cuenta;
using Trivo.Aplicacion.Interfaces.Servicios;
using Trivo.Aplicacion.Utilidades;

namespace Trivo.Aplicacion.Modulos.Administrador.Commands.InicioSesion;

internal sealed class InicioSesionAdminstradorCommandHandler(
    IAutenticacionServicio autenticacionServicio,
    IRepositorioAdministrador repositorioAdministrador,
    ILogger<InicioSesionAdminstradorCommandHandler> logger
    ) : ICommandHandler<InicioSesionAdminstradorCommand, TokenRespuestaDto>
{
    public async Task<ResultadoT<TokenRespuestaDto>> Handle(InicioSesionAdminstradorCommand request, CancellationToken cancellationToken)
    {
        var email = await repositorioAdministrador.BuscarPorEmailAsync(request.Email, cancellationToken);
        if (email is null)
        {
            logger.LogWarning("Inicio de sesión fallido: no se encontró administrador con el email '{Email}'.", request.Email);
        
            return ResultadoT<TokenRespuestaDto>.Fallo(Error.NoEncontrado("404", "Administrador no encontrado."));
        }

        if ( !BCrypt.Net.BCrypt.Verify(request.Contrasena, email.ContrasenaHash) )
        {
            logger.LogWarning("Inicio de sesión fallido: contraseña incorrecta para el administrador con email '{Email}'.", request.Email);
        
            return ResultadoT<TokenRespuestaDto>.Fallo(Error.Conflicto("409", "La contraseña es incorrecta."));
        }

        var tokenAcceso = await autenticacionServicio.GenerarToken(email, cancellationToken);
        var refrescarToken = autenticacionServicio.GenerarRefreshToken(email);

        logger.LogInformation("Inicio de sesión exitoso para el administrador con ID '{Id}' y email '{Email}'.", email.Id, email.Email);

        return ResultadoT<TokenRespuestaDto>.Exito(new TokenRespuestaDto
        {
            TokenAcceso = tokenAcceso,
            TokenRefresco = refrescarToken
        });
    }

}