using Microsoft.AspNetCore.SignalR;
using Trivo.Aplicacion.DTOs.Emparejamiento;
using Trivo.Aplicacion.Interfaces.Servicios.SignaIR;
using Trivo.Infraestructura.Compartido.SignalR.Hubs;

namespace Trivo.Infraestructura.Compartido.SignalR;

public class NotificadorDeEmparejamiento(IHubContext<EmparejamientoHub, IEmparejamientoHub> hubContext) : INotificadorDeEmparejamiento
{
    public async Task NotificarEmparejamiento(Guid usuarioId, IEnumerable<EmparejamientoDto> emparejamientos)
    {
        await hubContext.Clients.User(usuarioId.ToString())
            .RecibirEmparejamiento(emparejamientos);
    }

    public async Task NotificarNuevoEmparejamiento(Guid usuarioId, IEnumerable<EmparejamientoDto> emparejamientos)
    {
        await hubContext.Clients.User(usuarioId.ToString())
            .RecibirNuevoEmparejamiento(emparejamientos);
    }
}