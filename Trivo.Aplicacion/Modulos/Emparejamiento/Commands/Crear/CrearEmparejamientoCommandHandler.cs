using Microsoft.Extensions.Logging;
using Trivo.Aplicacion.Abstracciones.Mensajes;
using Trivo.Aplicacion.DTOs.Emparejamiento;
using Trivo.Aplicacion.Interfaces.Repositorio;
using Trivo.Aplicacion.Interfaces.Repositorio.Cuenta;
using Trivo.Aplicacion.Utilidades;
using Trivo.Dominio.Enum;

namespace Trivo.Aplicacion.Modulos.Emparejamiento.Commands.Crear;

internal sealed class CrearEmparejamientoCommandHandler(
    ILogger<CrearEmparejamientoCommandHandler> logger,
    IRepositorioEmparejamiento repositorioEmparejamiento,
    IRepositorioReclutador repositorioReclutador,
    IRepositorioExperto repositorioExperto
    ) : ICommandHandler<CrearEmparejamientoCommand, EmparejamientoDetallesDto>
{
   public async Task<ResultadoT<EmparejamientoDetallesDto>> Handle(CrearEmparejamientoCommand request, CancellationToken cancellationToken)
    {
        if (request is null)
        {
            logger.LogWarning("La solicitud de creación de emparejamiento es nula.");
            
            return ResultadoT<EmparejamientoDetallesDto>.Fallo(Error.Fallo("400", "La solicitud enviada no puede ser nula."));
        }

        var reclutador = await repositorioReclutador.ObtenerByIdAsync(request.ReclutadorId ?? Guid.Empty, cancellationToken);
        if (reclutador is null)
        {
            logger.LogWarning("No se encontró el reclutador con ID {ReclutadorId}.", request.ReclutadorId);
            
            return ResultadoT<EmparejamientoDetallesDto>.Fallo(Error.NoEncontrado("404", "El reclutador especificado no fue encontrado."));
        }

        var experto = await repositorioExperto.ObtenerByIdAsync(request.ExpertoId ?? Guid.Empty, cancellationToken);
        if (experto is null)
        {
            logger.LogWarning("No se encontró el experto con ID {ExpertoId}.", request.ExpertoId);
            
            return ResultadoT<EmparejamientoDetallesDto>.Fallo(Error.NoEncontrado("404", "El experto especificado no fue encontrado."));
        }
        
        var emparejamiento = new Dominio.Modelos.Emparejamiento
        {
            ReclutadorId = reclutador.Id,
            ExpertoId = experto.Id,
            EmparejamientoEstado = EmparejamientoEstado.Pendiente.ToString()
        };
        
        if (!Enum.TryParse(request.CreadoPor, ignoreCase: true, out Roles rolCreador) || !EstadosPorRol.TryGetValue(rolCreador, out var valor))
        {
            return ResultadoT<EmparejamientoDetallesDto>.Fallo(Error.Fallo("400", "Rol de creador inválido."));
        }

        var (estadoExperto, estadoReclutador) = valor;
        emparejamiento.ExpertoEstado = estadoExperto;
        emparejamiento.ReclutadorEstado = estadoReclutador;
        
        await repositorioEmparejamiento.CrearAsync(emparejamiento, cancellationToken);
        
        logger.LogInformation("Se creó un nuevo emparejamiento entre el reclutador {ReclutadorId} y el experto {ExpertoId}.",
            emparejamiento.ReclutadorId, emparejamiento.ExpertoId);

        EmparejamientoDetallesDto empresaDto = new
        (
            EmparejamientoId: emparejamiento.Id ?? Guid.Empty,
            ReclutadotId: emparejamiento.ReclutadorId ?? Guid.Empty,
            ExpertoId: emparejamiento.ExpertoId ?? Guid.Empty,
            ExpertoEstado: emparejamiento.ExpertoEstado ?? string.Empty,
            ReclutadorEstado: emparejamiento.ReclutadorEstado ?? string.Empty,
            EmparejamientoEstado: emparejamiento.EmparejamientoEstado ?? string.Empty,
            FechaRegistro: emparejamiento.FechaRegistro
        );
        
        return ResultadoT<EmparejamientoDetallesDto>.Exito(empresaDto);
    }

   #region Metodos Privados
   
    private static readonly Dictionary<Roles, (string expertoEstado, string reclutadorEstado)> EstadosPorRol =
       new()
       {
           { Roles.Experto, (ExpertoEstado.Match.ToString(), ReclutadorEstado.Pendiente.ToString()) },
           { Roles.Reclutador, (ExpertoEstado.Pendiente.ToString(), ReclutadorEstado.Match.ToString()) }
       };
   
   #endregion
}