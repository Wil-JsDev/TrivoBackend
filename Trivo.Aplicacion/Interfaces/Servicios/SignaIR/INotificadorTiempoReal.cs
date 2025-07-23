using Trivo.Aplicacion.DTOs.Chat;
using Trivo.Aplicacion.DTOs.Mensaje;
using Trivo.Aplicacion.Paginacion;

namespace Trivo.Aplicacion.Interfaces.Servicios.SignaIR;

public interface INotificadorTiempoReal
{
    Task NotificarMensajePrivado(MensajeDto mensaje, Guid usuarioId);
    Task NotificarMatchConfirmado(Guid usuarioId, string contenido);
    Task NotificarMatchPendiente(Guid usuarioId, string contenido);
    Task NotificarNuevoChat(Guid usuarioId, IEnumerable<ChatDto> chat);
    Task NotificarChatsPaginados(Guid usuarioId, ResultadoPaginado<ChatDto> chat);
    Task NotificarPaginaMensajes(Guid usuarioId, Guid chatId, ResultadoPaginado<MensajeDto> pagina);
}