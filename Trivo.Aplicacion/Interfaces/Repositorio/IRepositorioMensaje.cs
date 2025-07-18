using Trivo.Aplicacion.Paginacion;
using Trivo.Dominio.Modelos;

namespace Trivo.Aplicacion.Interfaces.Repositorio;

public interface IRepositorioMensaje: IRepositorioGenerico<Mensaje>
{
    Task<Mensaje?> ObtenerUltimoMensajePorChatIdAsync(Guid chatId, CancellationToken cancellationToken);
    Task<ResultadoPaginado<Mensaje>> ObtenerMensajePorChatIdPaginadoAsync(Guid chatId, int pagina, int tamano, CancellationToken cancellationToken);

}