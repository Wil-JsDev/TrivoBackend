using Trivo.Aplicacion.DTOs.Emparejamiento;

namespace Trivo.Aplicacion.Interfaces.Servicios.SignaIR;

public interface INotificadorDeEmparejamiento
{
    Task NotificarEmparejamiento(Guid reclutadorId, Guid expertoId,
        IEnumerable<EmparejamientoDto> emparejamientosReclutador,
        IEnumerable<EmparejamientoDto> emparejamientosExperto);

    Task NotificarNuevoEmparejamiento(Guid reclutadorId, Guid expertoId,
        IEnumerable<EmparejamientoDto> emparejamientosReclutador,
        IEnumerable<EmparejamientoDto> emparejamientosExperto);
}