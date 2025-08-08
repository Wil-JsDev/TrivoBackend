using Trivo.Aplicacion.DTOs.Emparejamiento;

namespace Trivo.Aplicacion.Interfaces.Servicios.SignaIR;

public interface IEmparejamientoHub
{
    Task RecibirEmparejamiento(IEnumerable<EmparejamientoDto> emparejamiento);
    
    Task RecibirNuevoEmparejamiento(IEnumerable<EmparejamientoDto> emparejamiento);
    
    Task RecibirEmparejamientoCompletado(Guid emparejamientoId,EmparejamientoDetallesDto emparejamiento);
    
    Task RecibirEmparejamientoRechazado(Guid emparejamientoId,EmparejamientoDetallesDto emparejamiento);
}