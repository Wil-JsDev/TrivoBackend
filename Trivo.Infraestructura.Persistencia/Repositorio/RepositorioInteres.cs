using Microsoft.EntityFrameworkCore;
using Trivo.Aplicacion.Interfaces.Repositorio;
using Trivo.Aplicacion.Paginacion;
using Trivo.Dominio.Modelos;
using Trivo.Infraestructura.Persistencia.Contexto;

namespace Trivo.Infraestructura.Persistencia.Repositorio;

public class RepositorioInteres(TrivoContexto trivoContexto) : IRepositorioInteres
{
    public async Task CrearInteresAsync(IEnumerable<Interes> interes, CancellationToken cancellationToken)
    {
        await trivoContexto.Set<Interes>().AddRangeAsync(interes, cancellationToken);
        await trivoContexto.SaveChangesAsync(cancellationToken);
    }

    public async Task ActualizarInteresesDeUsuarioAsync(Guid usuarioId, List<Guid> interesIds, CancellationToken cancellationToken)
    {
        var interesesActualesIds = await trivoContexto.Set<UsuarioInteres>()
            .Where(ui => ui.UsuarioId == usuarioId)
            .Select(ui => ui.InteresId!.Value)
            .ToListAsync(cancellationToken);

        var interesesAEliminar = interesesActualesIds.Except(interesIds);
        var interesesNuevos = interesIds.Except(interesesActualesIds);

        // Eliminar solo los que ya no están
        var relacionesAEliminar = await trivoContexto.Set<UsuarioInteres>()
            .Where(ui => ui.UsuarioId == usuarioId && interesesAEliminar.Contains(ui.InteresId!.Value))
            .ToListAsync(cancellationToken);

        trivoContexto.Set<UsuarioInteres>().RemoveRange(relacionesAEliminar);

        var relacionesANuevas = interesesNuevos.Select(id => new UsuarioInteres
        {
            UsuarioId = usuarioId,
            InteresId = id
        });

        await trivoContexto.Set<UsuarioInteres>().AddRangeAsync(relacionesANuevas, cancellationToken);
        
        await trivoContexto.SaveChangesAsync(cancellationToken);
    }
    
    public async Task<ResultadoPaginado<Interes>> ObtenerInteresesPaginadoAsync(int numeroPagina, int tamanoPagina, CancellationToken cancellationToken)
    {
        var total = await trivoContexto.Set<Interes>().AsNoTracking().CountAsync(cancellationToken);
            
        var interes = await trivoContexto.Set<Interes>().AsNoTracking()
            .Skip((numeroPagina - 1) * tamanoPagina)
            .Take(tamanoPagina)
            .ToListAsync(cancellationToken);

        return new ResultadoPaginado<Interes>(interes, total, numeroPagina, tamanoPagina);
    }

    public async Task<Interes> ObtenerPorIdAsync(Guid interesId, CancellationToken cancellationToken)
    {
        return (await trivoContexto.Set<Interes>()
            .AsNoTracking()
            .FirstOrDefaultAsync(interes => interes.Id == interesId, cancellationToken))!;
    }

    public async Task<IEnumerable<Usuario>> ObtenerUsuariosPorInteresesAsync(IEnumerable<Guid> interesesId, CancellationToken cancellationToken)
    {
        return await trivoContexto.Set<Usuario>()
            .AsNoTracking()
            .Include(u => u.UsuarioInteres)!
            .ThenInclude(us => us.Interes)
            .Include(u => u.UsuarioHabilidades)!
                .ThenInclude(us => us.Habilidad)
            .Where(u => u.UsuarioInteres!.Any(uh => interesesId.Contains(uh.InteresId!.Value)))
            .AsSplitQuery()
            .ToListAsync(cancellationToken);
    }
}