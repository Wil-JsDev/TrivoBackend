using Microsoft.Extensions.Logging;
using Serilog;
using Trivo.Aplicacion.Abstracciones.Mensajes;
using Trivo.Aplicacion.Interfaces.Repositorio.Cuenta;
using Trivo.Aplicacion.Utilidades;

namespace Trivo.Aplicacion.Modulos.Administrador.Commands.DesbanearUsuario;

internal sealed class DesbanearUsuarioCommandHandler(
    ILogger<DesbanearUsuarioCommandHandler> logger,
    IRepositorioAdministrador repositorioAdministrador,
    IRepositorioUsuario repositorioUsuario
    ) : ICommandHandler<DesbanearUsuarioCommand, string>
{
    public async Task<ResultadoT<string>> Handle(DesbanearUsuarioCommand request, CancellationToken cancellationToken)
    {
        var usuario = await repositorioUsuario.ObtenerByIdAsync(request.UsuarioId ?? Guid.Empty, cancellationToken);
        if (usuario is null)
        {
            logger.LogInformation("No se encontró el usuario con ID {UsuarioId}", request.UsuarioId);

            return ResultadoT<string>.Fallo(Error.NoEncontrado("404", "Este usuario no existe"));
        }
        
        await repositorioAdministrador.DesbanearUsuarioAsync(request.UsuarioId ?? Guid.Empty, cancellationToken);
        
        logger.LogInformation("El usuario con ID '{UsuarioId}' ha sido desbaneado.", usuario.Id);
        
        return ResultadoT<string>.Exito($"El usuario {usuario.Nombre} - {usuario.Id} ha sido desbaneado");
        
    }
}