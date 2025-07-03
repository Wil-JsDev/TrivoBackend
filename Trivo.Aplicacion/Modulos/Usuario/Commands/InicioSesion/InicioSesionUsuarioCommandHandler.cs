using Microsoft.Extensions.Logging;
using Trivo.Aplicacion.Abstracciones.Mensajes;
using Trivo.Aplicacion.DTOs.Cuentas.Usuarios;
using Trivo.Aplicacion.DTOs.JWT;
using Trivo.Aplicacion.Interfaces.Repositorio.Cuenta;
using Trivo.Aplicacion.Interfaces.Servicios;
using Trivo.Aplicacion.Utilidades;

namespace Trivo.Aplicacion.Modulos.Usuario.Commands.InicioSesion;

internal sealed class InicioSesionUsuarioCommandHandler(
    ILogger<InicioSesionUsuarioCommandHandler> logger,
    IRepositorioUsuario repositorioUsuarios,
    IAutenticacionServicio autenticacionServicio
    ) : ICommandHandler<InicioSesionUsuarioCommand, TokenRespuestaDto>
{
    public async Task<ResultadoT<TokenRespuestaDto>> Handle(InicioSesionUsuarioCommand request, CancellationToken cancellationToken)
    {
        if (request is null)
        {
            logger.LogWarning("Inicio de sesión fallido: solicitud nula");

            return ResultadoT<TokenRespuestaDto>.Fallo(Error.Fallo("400",
                "La solicitud de inicio de sesión es inválida"));
        }

        var emailUsuario = await repositorioUsuarios.BuscarPorEmailUsuarioAsync(request.Email, cancellationToken);
        if (emailUsuario is null)
        {
            logger.LogWarning("Inicio de sesión fallido: no se encontró usuario con email {Email}", request.Email);
            
            return ResultadoT<TokenRespuestaDto>.Fallo(Error.NoEncontrado("404", "Usuario no encontrado."));
        }

        if (!await repositorioUsuarios.CuentaConfirmadaAsync(emailUsuario.Id!.Value, cancellationToken))
        {
            logger.LogWarning("Inicio de sesión fallido: cuenta no confirmada para usuario con ID {UserId}", emailUsuario.Id);
            
            return ResultadoT<TokenRespuestaDto>.Fallo(Error.Conflicto("409", "La cuenta no está confirmada."));
        }

        if (!BCrypt.Net.BCrypt.Verify(request.Contrasena, emailUsuario.ContrasenaHash))
        {
            logger.LogWarning("Inicio de sesión fallido: contraseña incorrecta para usuario con ID {UserId}", emailUsuario.Id);
            
            return ResultadoT<TokenRespuestaDto>.Fallo(Error.Conflicto("409", "La contraseña es incorrecta."));
        }

        var tokenAcceso = await autenticacionServicio.GenerarToken(emailUsuario, cancellationToken);
        
        var refrescarToken =  autenticacionServicio.GenerarRefreshToken(emailUsuario);
        
        logger.LogInformation("Inicio de sesión exitoso para el usuario con ID {UserId}", emailUsuario.Id);

        return ResultadoT<TokenRespuestaDto>.Exito(new TokenRespuestaDto
        {
            TokenAcceso = tokenAcceso,
            TokenRefresco = refrescarToken
        });
        
    }
}