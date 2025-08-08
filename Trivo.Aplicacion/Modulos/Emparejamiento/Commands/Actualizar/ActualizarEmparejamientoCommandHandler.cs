using Microsoft.Extensions.Logging;
using Trivo.Aplicacion.Abstracciones.Mensajes;
using Trivo.Aplicacion.DTOs.Emparejamiento;
using Trivo.Aplicacion.Interfaces.Repositorio;
using Trivo.Aplicacion.Interfaces.Servicios.SignaIR;
using Trivo.Aplicacion.Utilidades;
using Trivo.Dominio.Enum;

namespace Trivo.Aplicacion.Modulos.Emparejamiento.Commands.Actualizar;

internal sealed class ActualizarEmparejamientoCommandHandler(
    ILogger<ActualizarEmparejamientoCommandHandler> logger,
    IRepositorioEmparejamiento emparejamientoRepositorio,
    INotificadorDeEmparejamiento emparejamientoNotificador
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
        
        if (request.FaltaPorEmparejamiento is FaltaPorEmparejamiento.Experto or FaltaPorEmparejamiento.Reclutador)
        {
            await emparejamientoRepositorio.ActualizarEstadoEmparejamientoAsync(request.EmparejamientoId, request.Estado, cancellationToken);

            var dto = MapearADetallesDto(emparejamiento);

            if (request.Estado is EstadoDeActualizacionEmparejamiento.Completado or EstadoDeActualizacionEmparejamiento.Rechazado)
            {
                await NotificarSegunEstadoAsync(request.UsuarioId, emparejamiento.Id ?? Guid.Empty, dto, request.Estado.Value);
            }

            logger.LogInformation("Emparejamiento {EmparejamientoId} actualizado por {FaltaPor}: nuevo estado {Estado}",
                emparejamiento.Id, request.FaltaPorEmparejamiento, request.Estado);

            return ResultadoT<EmparejamientoDetallesDto>.Exito(dto);
        }

        var resultadoDto = MapearADetallesDto(emparejamiento);
        
        return ResultadoT<EmparejamientoDetallesDto>.Exito(resultadoDto);
        
        #region solucion anterior

             // if (request.FaltaPorEmparejamiento == FaltaPorEmparejamiento.Experto)
        // {
        //     // emparejamiento.ExpertoEstado = request.Estado.ToString();
        //     // emparejamiento.ReclutadorEstado = request.Estado.ToString();
        //     await emparejamientoRepositorio.ActualizarEstadoEmparejamientoAsync(request.EmparejamientoId, request.Estado  ,cancellationToken);
        //     
        //     var resultadoDtoExperto = MapearADetallesDto(emparejamiento);
        //
        //     // if (request.Estado == EstadoDeActualizacionEmparejamiento.Completado)
        //     // {
        //     //    await emparejamientoNotificador.NotificarEmparejamientoCompletado(request.UsuarioId,
        //     //         emparejamiento.Id ?? Guid.Empty,
        //     //         resultadoDtoExperto);
        //     // }
        //     //
        //     // if (request.Estado == EstadoDeActualizacionEmparejamiento.Rechazado)
        //     // {
        //     //     await emparejamientoNotificador.NotificarEmparejamientoRechazado(request.UsuarioId,
        //     //         emparejamiento.Id ?? Guid.Empty,
        //     //         resultadoDtoExperto);
        //     // }
        //     
        //     if (request.Estado is EstadoDeActualizacionEmparejamiento.Completado or EstadoDeActualizacionEmparejamiento.Rechazado)
        //     {
        //         await NotificarSegunEstadoAsync(request.UsuarioId, emparejamiento.Id ?? Guid.Empty, resultadoDtoExperto, request.Estado.Value);
        //     }
        //     
        //     return ResultadoT<EmparejamientoDetallesDto>.Exito(resultadoDtoExperto);
        // }
        //
        // if (request.FaltaPorEmparejamiento == FaltaPorEmparejamiento.Reclutador)
        // {
        //     // emparejamiento.ReclutadorEstado = request.Estado.ToString();
        //     // emparejamiento.ExpertoEstado = request.Estado.ToString();
        //     // emparejamiento.EmparejamientoEstado = request.Estado.ToString();
        //     // emparejamiento.FechaActualizacion = DateTime.UtcNow;
        //     await emparejamientoRepositorio.ActualizarEstadoEmparejamientoAsync(request.EmparejamientoId, request.Estado  ,cancellationToken);
        //     
        //     var resultadoDtoReclutador = MapearADetallesDto(emparejamiento);
        //
        //     // if (request.Estado == EstadoDeActualizacionEmparejamiento.Completado)
        //     // {
        //     //     await emparejamientoNotificador.NotificarEmparejamientoCompletado(request.UsuarioId,
        //     //         emparejamiento.Id ?? Guid.Empty,
        //     //         resultadoDtoReclutador);
        //     // }
        //     //
        //     // if (request.Estado == EstadoDeActualizacionEmparejamiento.Rechazado)
        //     // {
        //     //     await emparejamientoNotificador.NotificarEmparejamientoRechazado(request.UsuarioId,
        //     //         emparejamiento.Id ?? Guid.Empty,
        //     //         resultadoDtoReclutador);
        //     // }
        //     
        //     if (request.Estado is EstadoDeActualizacionEmparejamiento.Completado or EstadoDeActualizacionEmparejamiento.Rechazado)
        //     {
        //         await NotificarSegunEstadoAsync(request.UsuarioId, emparejamiento.Id ?? Guid.Empty, resultadoDtoReclutador, request.Estado.Value);
        //     }
        //     
        //     return ResultadoT<EmparejamientoDetallesDto>.Exito(resultadoDtoReclutador);
        // }
        // var resultadoDtoGeneral = MapearADetallesDto(emparejamiento);
        //
        // return ResultadoT<EmparejamientoDetallesDto>.Exito(resultadoDtoGeneral);

        #endregion
       
    }

    #region Metodos privados
        private Task NotificarSegunEstadoAsync(Guid usuarioId, Guid emparejamientoId, EmparejamientoDetallesDto dto, EstadoDeActualizacionEmparejamiento estado)
        {
            return estado switch
            {
                EstadoDeActualizacionEmparejamiento.Completado => emparejamientoNotificador.NotificarEmparejamientoCompletado(usuarioId, emparejamientoId, dto),
                EstadoDeActualizacionEmparejamiento.Rechazado => emparejamientoNotificador.NotificarEmparejamientoRechazado(usuarioId, emparejamientoId, dto),
                _ => Task.CompletedTask
            };
        }
    
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