using Microsoft.Extensions.Logging;
using Trivo.Aplicacion.Abstracciones.Mensajes;
using Trivo.Aplicacion.DTOs.Emparejamiento;
using Trivo.Aplicacion.Interfaces.Repositorio;
using Trivo.Aplicacion.Interfaces.Repositorio.Cuenta;
using Trivo.Aplicacion.Interfaces.Servicios.SignaIR;
using Trivo.Aplicacion.Mapper;
using Trivo.Aplicacion.Utilidades;
using Trivo.Dominio.Enum;

namespace Trivo.Aplicacion.Modulos.Emparejamiento.Commands.Rechazos;

internal sealed class CrearRechazosEmparejamientoCommandHandler(
    ILogger<CrearRechazosEmparejamientoCommandHandler> logger,
    IRepositorioEmparejamiento repositorioEmparejamiento,
    IRepositorioReclutador repositorioReclutador,
    INotificadorDeEmparejamiento emparejamientoNotificador,
    IRepositorioExperto repositorioExperto
    ) : ICommandHandler<CrearRechazosEmparejamientoCommand, string>
{
    public async Task<ResultadoT<string>> Handle(CrearRechazosEmparejamientoCommand request, CancellationToken cancellationToken)
    {
        var reclutador = await repositorioReclutador.ObtenerIdAsync(request.ReclutadorId ?? Guid.Empty, cancellationToken);
        if (reclutador is null)
        {
            logger.LogWarning("No se encontró el reclutador con ID {ReclutadorId}.", request.ReclutadorId);
        
            return ResultadoT<string>.Fallo(Error.NoEncontrado("404", "El reclutador especificado no fue encontrado."));
        }

        var experto = await repositorioExperto.ObtenerIdAsync(request.ExpertoId ?? Guid.Empty, cancellationToken);
        if (experto is null)
        {
            logger.LogWarning("No se encontró el experto con ID {ExpertoId}.", request.ExpertoId);
        
            return ResultadoT<string>.Fallo(Error.NoEncontrado("404", "El experto especificado no fue encontrado."));
        }
    
        var emparejamiento = new Dominio.Modelos.Emparejamiento
        {
            Id = Guid.NewGuid(),
            ReclutadorId = reclutador.Id,
            ExpertoId = experto.Id,
            EmparejamientoEstado = EmparejamientoEstado.Rechazado.ToString()
        };
    
        if (!request.CreadoPor.HasValue || !EstadosPorRol.TryGetValue(request.CreadoPor.Value, out var valor))
        {
            logger.LogWarning("El rol de creador es inválido. Rol: {RolCreador}.", request.CreadoPor);
    
            return ResultadoT<string>.Fallo(Error.Fallo("400", "Rol de creador inválido."));
        }

        var (estadoExperto, estadoReclutador) = valor;
        emparejamiento.ExpertoEstado = estadoExperto;
        emparejamiento.ReclutadorEstado = estadoReclutador;
        
        await repositorioEmparejamiento.CrearAsync(emparejamiento, cancellationToken);
    
        var emparejamientoGuardado = await repositorioEmparejamiento.ObtenerPorIdAsync(emparejamiento.Id ?? Guid.Empty, cancellationToken);
        
        logger.LogInformation("Se registró el rechazo del emparejamiento entre el reclutador {ReclutadorId} y el experto {ExpertoId}.",
            reclutador.Id, experto.Id);
        
        var expertoMapeado = RecomendacionMapper.MappearAExpertoDto(
            emparejamientoGuardado.Experto!.Usuario!,
            experto
        );
        
        var reclutadorMapeado = RecomendacionMapper.MappearAReclutadorDto(
            emparejamientoGuardado.Reclutador!.Usuario!,
            reclutador
        );
        
        var emparejamientoDetallesDtoExperto = new EmparejamientoDto(
            EmparejamientoId: emparejamientoGuardado.Id ?? Guid.Empty,
            ReclutadotId: null,
            ExpertoId: experto.Id ?? Guid.Empty,
            EmparejamientoEstado: emparejamientoGuardado.EmparejamientoEstado ?? string.Empty,
            ExpertoEstado: emparejamientoGuardado.ExpertoEstado ?? string.Empty,
            ReclutadorEstado: emparejamientoGuardado.ReclutadorEstado ?? string.Empty,
            FechaRegistro: emparejamientoGuardado.FechaRegistro,
            ExpertoDto: expertoMapeado,
            ReclutadorDto: reclutadorMapeado
        );
        
        var emparejamientoDetallesDtoReclutador = new EmparejamientoDto(
            EmparejamientoId: emparejamientoGuardado.Id ?? Guid.Empty,
            ReclutadotId: null,
            ExpertoId: experto.Id ?? Guid.Empty,
            EmparejamientoEstado: emparejamientoGuardado.EmparejamientoEstado ?? string.Empty,
            ExpertoEstado: emparejamientoGuardado.ExpertoEstado ?? string.Empty,
            ReclutadorEstado: emparejamientoGuardado.ReclutadorEstado ?? string.Empty,
            FechaRegistro: emparejamientoGuardado.FechaRegistro,
            ExpertoDto: expertoMapeado,
            ReclutadorDto: reclutadorMapeado
        );
        
        await emparejamientoNotificador.NotificarNuevoEmparejamiento(
            reclutador.Id ?? Guid.Empty,
            experto.Id ?? Guid.Empty,
            new List<EmparejamientoDto> { emparejamientoDetallesDtoReclutador },
            new List<EmparejamientoDto> { emparejamientoDetallesDtoExperto }
        );
        
        // EmparejamientoDto emparejamientoDetallesDtoReclutador = new
        // (
        //     EmparejamientoId: emparejamientoGuardado!.Id ?? Guid.Empty,
        //     ReclutadotId: reclutador.Id ?? Guid.Empty,
        //     ExpertoId: null,
        //     EmparejamientoEstado: emparejamientoGuardado.EmparejamientoEstado ?? string.Empty,
        //     ExpertoEstado: emparejamientoGuardado.ExpertoEstado ?? string.Empty,
        //     ReclutadorEstado: emparejamientoGuardado.ReclutadorEstado ?? string.Empty,
        //     FechaRegistro: emparejamientoGuardado.FechaRegistro,
        //     UsuarioReconmendacionDto:  EmparejamientoMapper.MappearReclutadorReconmendacionDto(
        //         emparejamientoGuardado.Reclutador!.Usuario!,
        //         reclutador
        //     )
        // );
        //
        // EmparejamientoDto emparejamientoDetallesDtoExperto = new
        // (
        //     EmparejamientoId: emparejamientoGuardado.Id ?? Guid.Empty,
        //     ReclutadotId: null,
        //     ExpertoId: experto.Id ?? Guid.Empty,
        //     EmparejamientoEstado: emparejamientoGuardado.EmparejamientoEstado ?? string.Empty,
        //     ExpertoEstado: emparejamientoGuardado.ExpertoEstado ?? string.Empty,
        //     ReclutadorEstado: emparejamientoGuardado.ReclutadorEstado ?? string.Empty,
        //     FechaRegistro: emparejamientoGuardado.FechaRegistro,
        //     UsuarioReconmendacionDto:  EmparejamientoMapper.MappearExpertoReconmendacionDto(
        //         emparejamientoGuardado.Experto!.Usuario!,
        //         experto
        //     )
        // );
        //
         
        Console.WriteLine("Datos Reclutador:");
        logger.LogInformation("Datos {ReclutadorDto}", emparejamientoDetallesDtoReclutador);

        Console.WriteLine("Datos Experto:");
        logger.LogInformation("Datos {ExpertoDto}", emparejamientoDetallesDtoExperto);
        
        // await emparejamientoNotificador.NotificarNuevoEmparejamiento(
        //     reclutador.Id ?? Guid.Empty,
        //     experto.Id ?? Guid.Empty,
        //     new List<EmparejamientoDto> { emparejamientoDetallesDtoReclutador },
        //     new List<EmparejamientoDto> { emparejamientoDetallesDtoExperto }
        // );
        
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