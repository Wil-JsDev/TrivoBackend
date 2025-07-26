using Microsoft.Extensions.Logging;
using Trivo.Aplicacion.Abstracciones.Mensajes;
using Trivo.Aplicacion.Interfaces.Repositorio;
using Trivo.Aplicacion.Interfaces.Repositorio.Cuenta;
using Trivo.Aplicacion.Utilidades;
using Trivo.Dominio.Enum;

namespace Trivo.Aplicacion.Modulos.Emparejamiento.Commands.Rechazos;

internal sealed class CrearRechazosEmparejamientoCommandHandler(
    ILogger<CrearRechazosEmparejamientoCommandHandler> logger,
    IRepositorioEmparejamiento repositorioEmparejamiento,
    IRepositorioReclutador repositorioReclutador,
    IRepositorioExperto repositorioExperto
    ) : ICommandHandler<CrearRechazosEmparejamientoCommand, string>
{
    public async Task<ResultadoT<string>> Handle(CrearRechazosEmparejamientoCommand request, CancellationToken cancellationToken)
    {
        var reclutador = await repositorioReclutador.ObtenerByIdAsync(request.ReclutadorId ?? Guid.Empty, cancellationToken);
        if (reclutador is null)
        {
            logger.LogWarning("No se encontró el reclutador con ID {ReclutadorId}.", request.ReclutadorId);
        
            return ResultadoT<string>.Fallo(Error.NoEncontrado("404", "El reclutador especificado no fue encontrado."));
        }

        var experto = await repositorioExperto.ObtenerByIdAsync(request.ExpertoId ?? Guid.Empty, cancellationToken);
        if (experto is null)
        {
            logger.LogWarning("No se encontró el experto con ID {ExpertoId}.", request.ExpertoId);
        
            return ResultadoT<string>.Fallo(Error.NoEncontrado("404", "El experto especificado no fue encontrado."));
        }
    
        var emparejamiento = new Dominio.Modelos.Emparejamiento
        {
            ReclutadorId = reclutador.Id,
            ExpertoId = experto.Id,
            EmparejamientoEstado = EmparejamientoEstado.Rechazado.ToString()
        };
    
        await repositorioEmparejamiento.CrearAsync(emparejamiento, cancellationToken);
    
        logger.LogInformation("Se registró el rechazo del emparejamiento entre el reclutador {ReclutadorId} y el experto {ExpertoId}.",
            reclutador.Id, experto.Id);
    
        return ResultadoT<string>.Exito("El emparejamiento ha sido rechazado.");
    }

    
    #region Metodos Privados
   
     private static readonly Dictionary<Roles, (string expertoEstado, string reclutadorEstado)> EstadosPorRol =
        new()
        {
            { Roles.Experto, (ExpertoEstado.Rechazado.ToString(), ReclutadorEstado.Rechazado.ToString()) },
            { Roles.Reclutador, (ExpertoEstado.Rechazado.ToString(), ReclutadorEstado.Rechazado.ToString()) }
        };
   
    #endregion
}