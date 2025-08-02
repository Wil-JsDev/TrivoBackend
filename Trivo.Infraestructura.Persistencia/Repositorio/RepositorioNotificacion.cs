using Microsoft.EntityFrameworkCore;
using Trivo.Aplicacion.Interfaces.Repositorio;
using Trivo.Aplicacion.Paginacion;
using Trivo.Dominio.Modelos;
using Trivo.Infraestructura.Persistencia.Contexto;

namespace Trivo.Infraestructura.Persistencia.Repositorio;

public class RepositorioNotificacion(TrivoContexto trivoContexto) : RepositorioGenerico<Notificacion>(trivoContexto), IRepositorioNotificacion
{
    public async Task<ResultadoPaginado<Notificacion>> ObtenerNotificacionesPorUsuarioIdPaginadoAsync(Guid usuarioId, int numeroPagina, int tamanoPagina,
        CancellationToken cancellationToken)
    {
        var consulta = _trivoContexto.Set<Notificacion>()
            .AsNoTracking()
            .Include(n => n.Usuario)
            .Where(n => n.UsuarioId == usuarioId);
        
        var total = await consulta.CountAsync(cancellationToken);
        
        var notificaciones = await consulta.Skip((numeroPagina - 1) * tamanoPagina)
            .Take(tamanoPagina)
            .ToListAsync(cancellationToken);
        
        return new ResultadoPaginado<Notificacion>(notificaciones, total, numeroPagina, tamanoPagina);
    }

    public async Task<Notificacion?> ObtenerPorIdYUsuarioAsync(Guid notificacionId, Guid usuarioId, CancellationToken cancellationToken)
    {
        return await _trivoContexto.Set<Notificacion>()
            .AsNoTracking()
            .FirstOrDefaultAsync(n => 
                    n.NotificacionId == notificacionId && 
                    n.UsuarioId == usuarioId,
                cancellationToken);
    }
}