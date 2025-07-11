using Microsoft.Extensions.Logging;
using Trivo.Aplicacion.Abstracciones.Mensajes;
using Trivo.Aplicacion.Interfaces.Repositorio;
using Trivo.Aplicacion.Interfaces.Repositorio.Cuenta;
using Trivo.Aplicacion.Utilidades;

namespace Trivo.Aplicacion.Modulos.Habilidades.Commands.Actualizar;

internal sealed class ActualizarHabilidadCommandHandler(
    IRepositorioHabilidad repositorioHabilidad,
    ILogger<ActualizarHabilidadCommandHandler> logger,
    IRepositorioUsuario repositorioUsuario   
    ) : ICommandHandler<ActualizarHabilidadCommand, string>
{
    public async Task<ResultadoT<string>> Handle(ActualizarHabilidadCommand request, CancellationToken cancellationToken)
    {
        
        if (request.HabilidadIds.Count == 0)
        {
            logger.LogWarning("La lista de habilidades proporcionada está vacía o no fue enviada. UsuarioId: {UsuarioId}", request.UsuarioId);

            return ResultadoT<string>.Fallo(
                Error.Fallo("400", "Debe proporcionar al menos una habilidad para actualizar."));
        }
        
        var usuario = await repositorioUsuario.ObtenerByIdAsync(request.UsuarioId, cancellationToken);
        
        if (usuario is null)
        {
            logger.LogWarning("No se encontró el usuario con ID: {UsuarioId}", request.UsuarioId);
            
            return ResultadoT<string>.Fallo(Error.NoEncontrado("404", "El usuario no fue encontrado."));
        }

        await repositorioHabilidad.ActualizarHabilidadAsync(request.UsuarioId, request.HabilidadIds, cancellationToken);

        logger.LogInformation("Habilidades del usuario {UsuarioId} actualizadas correctamente.", request.UsuarioId);
        
        return ResultadoT<string>.Exito("Habilidades actualizadas correctamente.");
    }
}