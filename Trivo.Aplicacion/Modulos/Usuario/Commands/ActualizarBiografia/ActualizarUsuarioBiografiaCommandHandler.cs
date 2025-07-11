using Microsoft.Extensions.Logging;
using Trivo.Aplicacion.Abstracciones.Mensajes;
using Trivo.Aplicacion.Interfaces.Repositorio.Cuenta;
using Trivo.Aplicacion.Utilidades;

namespace Trivo.Aplicacion.Modulos.Usuario.Commands.ActualizarBiografia;

internal sealed class ActualizarUsuarioBiografiaCommandHandler(
    ILogger<ActualizarUsuarioBiografiaCommandHandler> logger,
    IRepositorioUsuario repositorioUsuario    
    ) : IQueryHandler<ActualizarUsuarioBiografiaCommand, string>
{
    public async Task<ResultadoT<string>> Handle(ActualizarUsuarioBiografiaCommand request, CancellationToken cancellationToken)
    {
        if (request is null)
        {
            logger.LogWarning("La solicitud para actualizar la biografía del usuario es nula.");
            
            return ResultadoT<string>.Fallo(Error.Fallo("400", "La solicitud no puede ser nula."));
        }

        var usuario = await repositorioUsuario.ObtenerByIdAsync(request.UsuarioId, cancellationToken);
        if (usuario is null)
        {
            logger.LogWarning("No se encontró un usuario con el ID: {UsuarioId}.", request.UsuarioId);
            
            return ResultadoT<string>.Fallo(Error.NoEncontrado("404", "El usuario especificado no fue encontrado."));
        }

        usuario.Biografia = request.Biografia;

        await repositorioUsuario.ActualizarAsync(usuario, cancellationToken);

        logger.LogInformation("La biografía del usuario con ID {UsuarioId} fue actualizada exitosamente.", request.UsuarioId);

        return ResultadoT<string>.Exito("Biografía actualizada correctamente.");
    }

}