using Microsoft.Extensions.Logging;
using Trivo.Aplicacion.Abstracciones.Mensajes;
using Trivo.Aplicacion.DTOs.Emparejamiento;
using Trivo.Aplicacion.Interfaces.Repositorio;
using Trivo.Aplicacion.Interfaces.Repositorio.Cuenta;
using Trivo.Aplicacion.Utilidades;
using Trivo.Dominio.Enum;

namespace Trivo.Aplicacion.Modulos.Emparejamiento.Commands.Actualizar;

internal sealed class ActualizarEmparejamientoCommandHandler(
    ILogger<ActualizarEmparejamientoCommandHandler> logger,
    IRepositorioEmparejamiento emparejamientoRepositorio
    ) : ICommandHandler<ActualizarEmparejamientoCommand, EmparejamientoDetallesDto>
{
    public async Task<ResultadoT<EmparejamientoDetallesDto>> Handle(ActualizarEmparejamientoCommand request, CancellationToken cancellationToken)
    {
        if (request.EmparejamientoId == Guid.Empty)
        {
            logger.LogWarning("Se recibió un EmparejamientoId vacío (Guid.Empty) en la solicitud");
    
            return ResultadoT<EmparejamientoDetallesDto>.Fallo(
                Error.Fallo("400", "El ID de emparejamiento es requerido y no puede estar vacío"));
        }
        
        var emparejamiento = await emparejamientoRepositorio.ObtenerPorIdAsync(request.EmparejamientoId, cancellationToken);
        if (emparejamiento is null)
        {
            logger.LogWarning("Emparejamiento con ID {EmparejamientoId} no encontrado.", request.EmparejamientoId);
            return ResultadoT<EmparejamientoDetallesDto>.Fallo(Error.NoEncontrado("404", "El emparejamiento no existe"));
        }
        
        if (request.FaltaPorEmparejamiento == FaltaPorEmparejamiento.Experto)
        {
            // emparejamiento.ExpertoEstado = request.Estado.ToString();
            // emparejamiento.ReclutadorEstado = request.Estado.ToString();
            await emparejamientoRepositorio.ActualizarEstadoEmparejamientoAsync(request.EmparejamientoId, request.Estado  ,cancellationToken);
            
            var resultadoDtoExperto = MapearADetallesDto(emparejamiento);
        
            return ResultadoT<EmparejamientoDetallesDto>.Exito(resultadoDtoExperto);
        }
        
        if (request.FaltaPorEmparejamiento == FaltaPorEmparejamiento.Reclutador)
        {
            // emparejamiento.ReclutadorEstado = request.Estado.ToString();
            // emparejamiento.ExpertoEstado = request.Estado.ToString();
            // emparejamiento.EmparejamientoEstado = request.Estado.ToString();
            // emparejamiento.FechaActualizacion = DateTime.UtcNow;
            await emparejamientoRepositorio.ActualizarEstadoEmparejamientoAsync(request.EmparejamientoId, request.Estado  ,cancellationToken);
            
            var resultadoDtoReclutador = MapearADetallesDto(emparejamiento);
        
            return ResultadoT<EmparejamientoDetallesDto>.Exito(resultadoDtoReclutador);
        }
        var resultadoDtoGeneral = MapearADetallesDto(emparejamiento);
        
        return ResultadoT<EmparejamientoDetallesDto>.Exito(resultadoDtoGeneral);
    }

    #region Metodos privados
    
        private EmparejamientoDetallesDto MapearADetallesDto(Dominio.Modelos.Emparejamiento emparejamiento)
        {
            return new EmparejamientoDetallesDto(
                EmparejamientoId: emparejamiento.Id ?? Guid.Empty,
                ReclutadotId: emparejamiento.ReclutadorId ?? Guid.Empty,
                ExpertoId: emparejamiento.ExpertoId ?? Guid.Empty,
                ExpertoEstado: emparejamiento.ExpertoEstado ?? string.Empty,
                ReclutadorEstado: emparejamiento.ReclutadorEstado ?? string.Empty,
                EmparejamientoEstado: emparejamiento.EmparejamientoEstado ?? string.Empty,
                FechaRegistro: emparejamiento.FechaRegistro
            );
        }

    #endregion
    
}