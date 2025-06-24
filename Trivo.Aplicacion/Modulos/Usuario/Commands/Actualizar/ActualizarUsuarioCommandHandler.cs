using Microsoft.Extensions.Logging;
using Trivo.Aplicacion.Abstracciones.Mensajes;
using Trivo.Aplicacion.DTOs.Cuentas.Usuarios;
using Trivo.Aplicacion.Interfaces.Repositorio.Cuenta;
using Trivo.Aplicacion.Utilidades;

namespace Trivo.Aplicacion.Modulos.Usuario.Commands.Actualizar;

internal sealed class ActualizarUsuarioCommandHandler(
    IRepositorioUsuario repositorioUsuario,
    ILogger<ActualizarUsuarioCommandHandler> logger
    
    ) : ICommandHandler<ActualizarUsuarioCommand, ActualizarUsuarioDto>
{
    public async Task<ResultadoT<ActualizarUsuarioDto>> Handle(
        ActualizarUsuarioCommand request, 
        CancellationToken cancellationToken)
    {
        if (request != null)
        {
            var usuario = await repositorioUsuario.ObtenerByIdAsync(request.UsuarioId, cancellationToken);
            if (usuario == null)
            {
                logger.LogWarning("No se encontró el usuario con ID '{RequestUsuarioId}'.", request.UsuarioId);
        
                return ResultadoT<ActualizarUsuarioDto>.Fallo(Error.NoEncontrado("404", "Usuario no encontrado."));
            }

            if (await repositorioUsuario.EmailEnUsoAsync(request.Email, request.UsuarioId, cancellationToken))
            {
                logger.LogWarning("El email '{Email}' ya está en uso por otro usuario.", request.Email);
        
                return ResultadoT<ActualizarUsuarioDto>.Fallo(Error.Conflicto("409", "Este email ya está en uso por otro usuario."));
            }

            if (await repositorioUsuario.NombreUsuarioEnUso(request.NombreUsuario, request.UsuarioId, cancellationToken))
            {
                logger.LogWarning("El nombre de usuario '{NombreUsuario}' ya está en uso por otro usuario.", request.NombreUsuario);
        
                return ResultadoT<ActualizarUsuarioDto>.Fallo(Error.Conflicto("409", "Este nombre de usuario ya está en uso por otro usuario."));
            }

            usuario.NombreUsuario = request.NombreUsuario;
            usuario.Email = request.Email;

            await repositorioUsuario.ActualizarAsync(usuario, cancellationToken);

            logger.LogInformation("Usuario con ID '{UsuarioId}' actualizado correctamente.", usuario.Id);

            ActualizarUsuarioDto actualizarUsuario = new
            (
                NombreUsuario: usuario.NombreUsuario,
                Email: usuario.Email
            );

            return ResultadoT<ActualizarUsuarioDto>.Exito(actualizarUsuario);
        }

        logger.LogWarning("Se recibió una solicitud nula en el comando ActualizarUsuarioCommand.");

        return ResultadoT<ActualizarUsuarioDto>.Fallo(Error.Fallo("400", "La solicitud no puede ser nula."));
    }
}