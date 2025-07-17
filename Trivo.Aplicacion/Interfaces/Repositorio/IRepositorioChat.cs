using Trivo.Aplicacion.Paginacion;
using Trivo.Dominio.Modelos;

namespace Trivo.Aplicacion.Interfaces.Repositorio;

public interface IRepositorioChat : IRepositorioGenerico<Chat>
{
    Task<IEnumerable<Chat>> Obtener10ChatsRecientes(Guid usuarioId, CancellationToken cancellationToken);
    Task<bool> ExisteAsync(Guid chatId, CancellationToken cancellationToken);
    Task<ResultadoPaginado<Chat>> ObtenerChatsPorUsuarioIdPaginadoAsync(Guid usuarioId, int pagina, int tamano, CancellationToken cancellationToken);
    Task<Chat> ObtenerChatPorIdAsync(Guid chatId, CancellationToken cancellationToken);

    Task<bool> UsuarioPerteneceAlChatAsync(Guid chatId, Guid usuarioId, CancellationToken cancellationToken);
    Task<Chat?> BuscarChat1a1Async(Guid emisorId, Guid receptorId, CancellationToken cancellationToken);

}