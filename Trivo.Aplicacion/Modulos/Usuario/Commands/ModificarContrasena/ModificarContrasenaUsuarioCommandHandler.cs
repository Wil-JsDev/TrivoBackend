using Microsoft.Extensions.Logging;
using Trivo.Aplicacion.Abstracciones.Mensajes;
using Trivo.Aplicacion.Interfaces.Repositorio.Cuenta;
using Trivo.Aplicacion.Utilidades;

namespace Trivo.Aplicacion.Modulos.Usuario.Commands.ModificarContrasena;

internal sealed class ModificarContrasenaUsuarioCommandHandler(
    ILogger<ModificarContrasenaUsuarioCommandHandler> logger,
    IRepositorioUsuario repositorioUsuario,
    IRepositorioCodigo repositorioCodigo
    ) : ICommandHandler<ModificarContrasenaUsuarioCommand, string>
{
    public async Task<ResultadoT<string>> Handle(
        ModificarContrasenaUsuarioCommand request, 
        CancellationToken cancellationToken
    )
    {
        if (request == null)
        {
            logger.LogWarning("La solicitud para actualizar la contraseña es nula.");
            
            return ResultadoT<string>.Fallo(Error.Fallo("400", "La solicitud no puede ser nula."));
        }

        var usuario = await repositorioUsuario.BuscarPorEmailUsuarioAsync(request.Email, cancellationToken);
        
        if (usuario == null)
        {
            logger.LogWarning("No se encontró el usuario con ID '{RequestUsuarioId}'.", usuario!.Id);
            
            return ResultadoT<string>.Fallo(Error.NoEncontrado("404", "El usuario no fue encontrado."));
        }
        
        var nuevaContrasena = BCrypt.Net.BCrypt.HashPassword(request.ConfirmacionDeContrsena);

        await repositorioUsuario.ActualizarContrasenaAsync(usuario, nuevaContrasena, cancellationToken);

        logger.LogInformation("La contraseña del usuario con ID '{UsuarioId}' fue actualizada exitosamente.", usuario.Id);

        return ResultadoT<string>.Exito("La contraseña ha sido actualizada correctamente.");
    }
}