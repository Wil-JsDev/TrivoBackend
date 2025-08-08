using Microsoft.Extensions.Logging;
using Serilog;
using Trivo.Aplicacion.Abstracciones.Mensajes;
using Trivo.Aplicacion.Interfaces.Repositorio.Cuenta;
using Trivo.Aplicacion.Utilidades;

namespace Trivo.Aplicacion.Modulos.Administrador.Commands.BanearUsuarios;

internal sealed class BanearUsuariosCommandHandler(
    ILogger<BanearUsuariosCommandHandler> logger,
    IRepositorioAdministrador repositorioAdministrador,
    IRepositorioUsuario repositorioUsuario
    ) : ICommandHandler<BanearUsuariosCommand, string>
{
    public async Task<ResultadoT<string>> Handle(BanearUsuariosCommand request, CancellationToken cancellationToken)
    {
        var usuario = await repositorioUsuario.ObtenerByIdAsync(request.UsuarioId, cancellationToken);
        if (usuario is null)
        {
            logger.LogInformation("No se encontró el usuario con ID {UsuarioId}", request.UsuarioId);

            return ResultadoT<string>.Fallo(Error.NoEncontrado("404", "Este usuario no existe"));
        }
        
        await repositorioAdministrador.BanearUsuarioAsync(request.UsuarioId, cancellationToken);
        
        logger.LogInformation("El usuario con ID '{UsuarioId}' ha sido baneado.", usuario.Id);
        
        return ResultadoT<string>.Exito($"El usuario {usuario.Nombre} - {usuario.Id} ha sido baneado");
    }
}