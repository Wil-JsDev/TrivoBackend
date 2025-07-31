using Microsoft.Extensions.Logging;
using Serilog;
using Trivo.Aplicacion.Abstracciones.Mensajes;
using Trivo.Aplicacion.Interfaces.Repositorio.Cuenta;
using Trivo.Aplicacion.Utilidades;

namespace Trivo.Aplicacion.Modulos.Usuario.Commands.ActualizarContrasenaAntigua;

internal sealed class ActualizarContrasenaAntiguaUsuarioCommandHandler(
    ILogger<ActualizarContrasenaAntiguaUsuarioCommandHandler> logger,
    IRepositorioUsuario repositorioUsuario
    )
    : ICommandHandler<ActualizarContrasenaAntiguaUsuarioCommand, string>
{
    public async Task<ResultadoT<string>> Handle(ActualizarContrasenaAntiguaUsuarioCommand request, CancellationToken cancellationToken)
    {
        var usuario = await repositorioUsuario.ObtenerByIdAsync(request.UsuarioId, cancellationToken);
        if (usuario is null)
        {
            logger.LogWarning("No se encontró el usuario con ID '{UsuarioId}' al intentar actualizar la contraseña.", request.UsuarioId);
            
            return ResultadoT<string>.Fallo(Error.NoEncontrado("404", "El usuario no existe"));
        }

        if (!BCrypt.Net.BCrypt.Verify(request.ContrasenaAntigua, usuario.ContrasenaHash))
        {
            logger.LogWarning("Actualización de contraseña fallida: la contraseña antigua no coincide para el usuario con ID '{UsuarioId}'.", usuario.Id);
            
            return ResultadoT<string>.Fallo(Error.Conflicto("409", "La contraseña antigua es incorrecta."));
        }

        var nuevaContrasena = BCrypt.Net.BCrypt.HashPassword(request.ConfirmacionDeContrsena);
        await repositorioUsuario.ActualizarContrasenaAsync(usuario, nuevaContrasena, cancellationToken);

        logger.LogInformation("Contraseña actualizada correctamente para el usuario con ID '{UsuarioId}'.", usuario.Id);

        return ResultadoT<string>.Exito("Su contraseña ha sido actualizada correctamente.");
    }

}