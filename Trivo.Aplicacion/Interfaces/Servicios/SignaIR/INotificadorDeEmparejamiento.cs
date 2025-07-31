using Trivo.Aplicacion.DTOs.Emparejamiento;

namespace Trivo.Aplicacion.Interfaces.Servicios.SignaIR;

public interface INotificadorDeEmparejamiento
{
    Task NotificarEmparejamiento(Guid usuarioId, IEnumerable<EmparejamientoDto> emparejamientos);
    
    Task NotificarNuevoEmparejamiento(Guid usuarioId, IEnumerable<EmparejamientoDto> emparejamientos);
}