using Microsoft.AspNetCore.SignalR;
using Trivo.Aplicacion.DTOs.Emparejamiento;
using Trivo.Aplicacion.Interfaces.Servicios.SignaIR;
using Trivo.Infraestructura.Compartido.SignalR.Hubs;

namespace Trivo.Infraestructura.Compartido.SignalR;

public class NotificadorDeEmparejamiento(IHubContext<EmparejamientoHub, IEmparejamientoHub> hubContext) : INotificadorDeEmparejamiento
{
    public async Task NotificarEmparejamiento(Guid reclutadorId, Guid expertoId,
        IEnumerable<EmparejamientoDto> emparejamientosReclutador,
        IEnumerable<EmparejamientoDto> emparejamientosExperto)
    {
        if (reclutadorId == expertoId)
        {
            await hubContext.Clients.User(reclutadorId.ToString())
                .RecibirEmparejamiento(emparejamientosReclutador);
        }
        else
        {
            await hubContext.Clients.User(reclutadorId.ToString())
                .RecibirEmparejamiento(emparejamientosReclutador);

            await hubContext.Clients.User(expertoId.ToString())
                .RecibirEmparejamiento(emparejamientosExperto);
        }
    }

    public async Task NotificarNuevoEmparejamiento(Guid reclutadorId, Guid expertoId,
        IEnumerable<EmparejamientoDto> emparejamientosReclutador,
        IEnumerable<EmparejamientoDto> emparejamientosExperto)
    {
        if (reclutadorId == expertoId)
        {
            await hubContext.Clients.User(reclutadorId.ToString())
                .RecibirNuevoEmparejamiento(emparejamientosReclutador);
        }
        else
        {
            await hubContext.Clients.User(reclutadorId.ToString())
                .RecibirNuevoEmparejamiento(emparejamientosReclutador);

            await hubContext.Clients.User(expertoId.ToString())
                .RecibirNuevoEmparejamiento(emparejamientosExperto);
        }
    }
    public async Task NotificarEmparejamientoCompletado(Guid usuarioId, Guid emparejamientoId, EmparejamientoDetallesDto emparejamientoDto)
    {
        await hubContext.Clients.User(usuarioId.ToString())
            .RecibirEmparejamientoCompletado(emparejamientoId,emparejamientoDto);
    }

    public async Task NotificarEmparejamientoRechazado(Guid usuarioId, Guid emparejamientoId, EmparejamientoDetallesDto emparejamientoDto)
    {
        await hubContext.Clients.User(usuarioId.ToString())
            .RecibirEmparejamientoRechazado(emparejamientoId,emparejamientoDto);   
    }
    
}