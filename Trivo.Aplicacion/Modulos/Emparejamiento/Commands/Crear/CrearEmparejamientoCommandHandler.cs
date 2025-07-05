using Microsoft.Extensions.Logging;
using Trivo.Aplicacion.Abstracciones.Mensajes;
using Trivo.Aplicacion.DTOs.Emparejamiento;
using Trivo.Aplicacion.Interfaces.Repositorio;
using Trivo.Aplicacion.Utilidades;
using Trivo.Dominio.Enum;

namespace Trivo.Aplicacion.Modulos.Emparejamiento.Commands.Crear;

internal sealed class CrearEmparejamientoCommandHandler(
    ILogger<CrearEmparejamientoCommandHandler> logger,
    IRepositorioEmparejamiento repositorioEmparejamiento,
    IRepositorioReclutador repositorioReclutador,
    IRepositorioExperto repositorioExperto
    ) : ICommandHandler<CrearEmparejamientoCommand, EmparejamientoDto>
{
   public async Task<ResultadoT<EmparejamientoDto>> Handle(CrearEmparejamientoCommand request, CancellationToken cancellationToken)
    {
        if (request is null)
        {
            logger.LogWarning("La solicitud de creación de emparejamiento es nula.");
            
            return ResultadoT<EmparejamientoDto>.Fallo(Error.Fallo("400", "La solicitud enviada no puede ser nula."));
        }

        var reclutador = await repositorioReclutador.ObtenerByIdAsync(request.ReclutadorId ?? Guid.Empty, cancellationToken);
        if (reclutador is null)
        {
            logger.LogWarning("No se encontró el reclutador con ID {ReclutadorId}.", request.ReclutadorId);
            
            return ResultadoT<EmparejamientoDto>.Fallo(Error.NoEncontrado("404", "El reclutador especificado no fue encontrado."));
        }

        var experto = await repositorioExperto.ObtenerByIdAsync(request.ExpertoId ?? Guid.Empty, cancellationToken);
        if (experto is null)
        {
            logger.LogWarning("No se encontró el experto con ID {ExpertoId}.", request.ExpertoId);
            
            return ResultadoT<EmparejamientoDto>.Fallo(Error.NoEncontrado("404", "El experto especificado no fue encontrado."));
        }

        Dominio.Modelos.Emparejamiento emparejamiento = new()
        {
            ReclutadorId = reclutador.Id,
            ExpertoId = experto.Id,
            ExpertoEstado = nameof(ExpertoEstado.Match),
            ReclutadorEstado = nameof(ReclutadorEstado.Match),
            EmparejamientoEstado = nameof(EmparejamientoEstado.Completado)
        };

        await repositorioEmparejamiento.CrearAsync(emparejamiento, cancellationToken);

        logger.LogInformation("Se creó un nuevo emparejamiento entre el reclutador {ReclutadorId} y el experto {ExpertoId}.",
            emparejamiento.ReclutadorId, emparejamiento.ExpertoId);

        EmparejamientoDto emparejamientoDto = new
        (
            EmparejamientoId: emparejamiento.Id!.Value,
            emparejamiento.ReclutadorId!.Value,
            emparejamiento.ExpertoId!.Value,
            emparejamiento.ExpertoEstado!,
            emparejamiento.ReclutadorEstado!,
            emparejamiento.EmparejamientoEstado!
        );
        
        return ResultadoT<EmparejamientoDto>.Exito(emparejamientoDto);
    }

}