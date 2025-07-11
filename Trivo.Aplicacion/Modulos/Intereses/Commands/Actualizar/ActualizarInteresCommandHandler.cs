using Microsoft.Extensions.Logging;
using Trivo.Aplicacion.Abstracciones.Mensajes;
using Trivo.Aplicacion.Interfaces.Repositorio;
using Trivo.Aplicacion.Interfaces.Repositorio.Cuenta;
using Trivo.Aplicacion.Utilidades;

namespace Trivo.Aplicacion.Modulos.Intereses.Commands.Actualizar;

internal sealed class ActualizarInteresCommandHandler(
    ILogger<ActualizarInteresCommandHandler> logger,
    IRepositorioInteres repositorioInteres,
    IRepositorioUsuario repositorioUsuario  
    ) : ICommandHandler<ActualizarInteresCommand, string>
{
    public async Task<ResultadoT<string>> Handle(ActualizarInteresCommand request, CancellationToken cancellationToken)
    {
        if (request.InteresIds.Count == 0)
        {
            logger.LogWarning("La lista de intereses proporcionada está vacía o no fue enviada. UsuarioId: {UsuarioId}", request.UsuarioId);

            return ResultadoT<string>.Fallo(
                Error.Fallo("400", "Debe proporcionar al menos un interes para actualizar."));
        }
        
        var usuario = await repositorioUsuario.ObtenerByIdAsync(request.UsuarioId, cancellationToken);
        
        if (usuario is null)
        {
            logger.LogWarning("No se encontró el usuario con ID: {UsuarioId}", request.UsuarioId);
            
            return ResultadoT<string>.Fallo(Error.NoEncontrado("404", "El usuario no fue encontrado."));
        }
        
        await repositorioInteres.ActualizarInteresesDeUsuarioAsync(request.UsuarioId, request.InteresIds, cancellationToken);

        logger.LogInformation("Intereses del usuario {UsuarioId} actualizados correctamente.", request.UsuarioId);
        
        return ResultadoT<string>.Exito("Intereses actualizados correctamente.");
    }
}