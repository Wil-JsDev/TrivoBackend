using Microsoft.AspNetCore.SignalR;
using Trivo.Aplicacion.DTOs.Cuentas.Usuarios;
using Trivo.Aplicacion.Interfaces.Servicios.SignaIR;
using Trivo.Infraestructura.Compartido.SignalR.Hubs;

namespace Trivo.Infraestructura.Compartido.SignalR;

public class NotificadorIA(IHubContext<RecomendacionUsuariosHub, IRecomendacionUsuariosHub> hubContext) : INotificadorIA
{
    public async Task NotificarRecomendaciones(Guid usuarioId, IEnumerable<UsuarioReconmendacionDto> recomendaciones)
    {
        Console.WriteLine($"📢 Notificando a usuario {usuarioId} con {recomendaciones.Count()} recomendaciones.");
        await hubContext.Clients.User(usuarioId.ToString())
            .RecibirRecomendaciones(recomendaciones);
    }
}