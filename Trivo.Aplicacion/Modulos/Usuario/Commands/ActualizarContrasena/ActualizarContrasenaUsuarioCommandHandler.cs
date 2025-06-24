using Microsoft.Extensions.Logging;
using Trivo.Aplicacion.Abstracciones.Mensajes;
using Trivo.Aplicacion.Interfaces.Repositorio.Cuenta;
using Trivo.Aplicacion.Interfaces.Servicios;
using Trivo.Aplicacion.Utilidades;

namespace Trivo.Aplicacion.Modulos.Usuario.Commands.ActualizarContrasena;

internal sealed class ActualizarContrasenaUsuarioCommandHandler(
    IRepositorioUsuario repositorioUsuario,
    ILogger<ActualizarContrasenaUsuarioCommandHandler> logger,
    ICodigoServicio codigoServicio
    ) : ICommandHandler<ActualizarContrasenaUsuarioCommand, string>
{
    public async Task<ResultadoT<string>> Handle(
        ActualizarContrasenaUsuarioCommand request, 
        CancellationToken cancellationToken)
    {

        if (request != null)
        {
            var usuario = await repositorioUsuario.ObtenerByIdAsync(request.UsuarioId ?? Guid.Empty, cancellationToken);
            if (usuario == null)
            {
                logger.LogWarning("No se encontró el usuario con ID '{RequestUsuarioId}'", request.UsuarioId);

                return ResultadoT<string>.Fallo(Error.NoEncontrado("404", "El usuario no fue encontrado."));
            }

            var codigoDisponible = await codigoServicio.CodigoDisponibleAsync(request.Codigo, cancellationToken);
            if (!codigoDisponible.EsExitoso)
            {
                logger.LogWarning("El código de verificación '{Codigo}' no es válido o ha expirado.", request.Codigo);

                return ResultadoT<string>.Fallo(Error.Conflicto("409", "El código ingresado no es válido o ha expirado."));
            }

            logger.LogInformation("Código verificado correctamente para el usuario '{UsuarioId}'. Procediendo a actualizar contraseña.", usuario.Id);

            var contrasenaHash = BCrypt.Net.BCrypt.HashPassword(request.ConfirmacionDeContrsena);

            await repositorioUsuario.ActualizarContrasenaAsync(usuario, contrasenaHash, cancellationToken);

            logger.LogInformation("Contraseña actualizada exitosamente para el usuario '{UsuarioId}'.", usuario.Id);

            return ResultadoT<string>.Exito("Contraseña actualizada correctamente.");
        }

        logger.LogWarning("Se recibió un comando CambiarContrasenaCommand nulo.");

        return ResultadoT<string>.Fallo(Error.Fallo("400", "La solicitud no puede ser nula."));

    }
}