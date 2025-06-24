using Microsoft.Extensions.Logging;
using Trivo.Aplicacion.Abstracciones.Mensajes;
using Trivo.Aplicacion.DTOs.Email;
using Trivo.Aplicacion.Interfaces.Repositorio.Cuenta;
using Trivo.Aplicacion.Interfaces.Servicios;
using Trivo.Aplicacion.Utilidades;

namespace Trivo.Aplicacion.Modulos.Usuario.Commands.OlvidarContrsena;

internal sealed class OlvidarContrasenaUsuarioCommandHandler(
    IRepositorioUsuario repositorioUsuario,
    ILogger<OlvidarContrasenaUsuarioCommandHandler> logger,
    ICodigoServicio codigoServicio,
    IEmailServicio emailServicio
    ) : ICommandHandler<OlvidarContrasenaUsuarioCommand, string>
{
    public async Task<ResultadoT<string>> Handle(
        OlvidarContrasenaUsuarioCommand request, 
        CancellationToken cancellationToken)
    {
        if (request != null)
        {
            var usuario = await repositorioUsuario.ObtenerByIdAsync(request.UsuarioId, cancellationToken);
            if (usuario == null)
            {
                logger.LogError("No se encontró el usuario con ID '{RequestUsuarioId}'", request.UsuarioId);

                return ResultadoT<string>.Fallo(Error.NoEncontrado("404", "Usuario no encontrado"));
            }

            var codigo = await codigoServicio.GenerarCodigoAsync(request.UsuarioId, cancellationToken);

            await emailServicio.EnviarEmailAsync(
                new EmailRespuestaDto(
                    Usuario: usuario.Email!,
                    Cuerpo: EmailTemas.OlvidarContrasena(codigo.Valor, usuario.NombreUsuario!),
                    Tema: "Olvidaste la contraseña"
                )
            );

            logger.LogInformation("Se envió el código de recuperación al correo '{Email}' para el usuario '{UsuarioId}'.",
                usuario.Email, usuario.Id);

            return ResultadoT<string>.Exito("Se envió el código de recuperación correctamente.");
        }

        logger.LogWarning("Se recibió un comando OlvidarContrasenaCommand nulo.");

        return ResultadoT<string>.Fallo(Error.Fallo("400", "La solicitud no puede ser nula."));

    }
}