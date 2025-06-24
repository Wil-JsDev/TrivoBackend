using Microsoft.Extensions.Logging;
using Trivo.Aplicacion.Abstracciones.Mensajes;
using Trivo.Aplicacion.Interfaces.Repositorio.Cuenta;
using Trivo.Aplicacion.Interfaces.Servicios;
using Trivo.Aplicacion.Utilidades;

namespace Trivo.Aplicacion.Modulos.Usuario.Commands.ConfirmarUsuario;

internal sealed class ConfirmarUsuarioCommandHandler(
    ILogger<ConfirmarUsuarioCommandHandler> logger,
    IRepositorioUsuario repositorioUsuario,
    ICodigoServicio codigoServicio
    ) : ICommandHandler<ConfirmarUsuarioCommand, string>
{
    public async Task<ResultadoT<string>> Handle(ConfirmarUsuarioCommand request, CancellationToken cancellationToken)
    {

        if (request == null)
        {
            logger.LogError("La solicitud para confirmar cuenta es nula.");
            
            return ResultadoT<string>.Fallo(Error.Fallo("400", "No se encontró el código del usuario."));
        }

        var usuario = await repositorioUsuario.ObtenerByIdAsync(request.UsuarioId, cancellationToken);
        if (usuario == null)
        {
            logger.LogWarning("No se encontró el usuario con ID '{RequestUsuarioId}'.", request.UsuarioId);
            
            return ResultadoT<string>.Fallo(Error.NoEncontrado("404", "Usuario no encontrado."));
        }

        var confirmarUsuario = await codigoServicio.ConfirmarCuentaAsync(usuario.Id ?? Guid.Empty, request.Codigo, cancellationToken);
        if (!confirmarUsuario.EsExitoso)
        {
            logger.LogWarning("Error al confirmar la cuenta del usuario con ID '{UsuarioId}': {MensajeError}", usuario.Id, confirmarUsuario.Error);
            
            return ResultadoT<string>.Fallo(confirmarUsuario.Error!);
        }

        logger.LogInformation("La cuenta del usuario con ID '{UsuarioId}' ha sido confirmada exitosamente.", usuario.Id);
        
        return ResultadoT<string>.Exito("La cuenta ha sido confirmada correctamente.");

    }
}