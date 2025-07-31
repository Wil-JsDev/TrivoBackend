using Microsoft.Extensions.Logging;
using Trivo.Aplicacion.Abstracciones.Mensajes;
using Trivo.Aplicacion.DTOs.Cuentas.Usuarios;
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
    
        logger.LogInformation("Se registró el rechazo del emparejamiento entre el reclutador {ReclutadorId} y el experto {ExpertoId}.",
            reclutador.Id, experto.Id);
        
        EmparejamientoDto emparejamientoDetallesDtoReclutador = new
        (
            EmparejamientoId: emparejamiento.Id ?? Guid.Empty,
            UsuarioReconmendacionDto: new List<UsuarioReconmendacionDto> { UsuarioMapper.MapToDto(reclutador.Usuario!) }
        );
        
        EmparejamientoDto emparejamientoDetallesDtoExperto = new
        (
            EmparejamientoId: emparejamiento.Id ?? Guid.Empty,
            UsuarioReconmendacionDto: new List<UsuarioReconmendacionDto> { UsuarioMapper.MapToDto(experto.Usuario!) }
        );
        
        await emparejamientoNotificador.NotificarNuevoEmparejamiento(
            reclutador.Id ?? Guid.Empty,
            experto.Id ?? Guid.Empty,
            new List<EmparejamientoDto> { emparejamientoDetallesDtoReclutador },
            new List<EmparejamientoDto> { emparejamientoDetallesDtoExperto }
        );
        
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