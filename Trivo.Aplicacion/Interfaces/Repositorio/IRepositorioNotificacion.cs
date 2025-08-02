using Trivo.Aplicacion.Paginacion;
using Trivo.Dominio.Modelos;

namespace Trivo.Aplicacion.Interfaces.Repositorio;

public interface IRepositorioNotificacion : IRepositorioGenerico<Notificacion>
{
    Task<ResultadoPaginado<Notificacion>> ObtenerNotificacionesPorUsuarioIdPaginadoAsync(Guid usuarioId,
        int numeroPagina,
        int tamanoPagina,
        CancellationToken cancellationToken);

    Task<Notificacion?> ObtenerPorIdYUsuarioAsync(
        Guid notificacionId,
        Guid usuarioId,
        CancellationToken cancellationToken);
}