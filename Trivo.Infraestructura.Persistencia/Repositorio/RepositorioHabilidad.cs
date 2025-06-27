using Microsoft.EntityFrameworkCore;
using Trivo.Aplicacion.Interfaces.Repositorio;
using Trivo.Aplicacion.Paginacion;
using Trivo.Dominio.Modelos;
using Trivo.Infraestructura.Persistencia.Contexto;

namespace Trivo.Infraestructura.Persistencia.Repositorio;

public class RepositorioHabilidad(TrivoContexto trivoContexto) : IRepositorioHabilidad
{
    public async Task CrearHabilidadAsync(Habilidad habilidades, CancellationToken cancellationToken)
    {
        await trivoContexto.Set<Habilidad>().AddAsync(habilidades, cancellationToken);
        await trivoContexto.SaveChangesAsync(cancellationToken);
    }

    public async Task ActualizarHabilidadAsync(Guid usuarioId, List<Guid> habilidadIds ,CancellationToken cancellationToken)
    {
        var interesesActualesIds = await trivoContexto.Set<UsuarioHabilidad>()
            .Where(ui => ui.UsuarioId == usuarioId)
            .Select(ui => ui.HabilidadId!.Value)
            .ToListAsync(cancellationToken);

        var interesesAEliminar = interesesActualesIds.Except(habilidadIds);
        
        var interesesNuevos = habilidadIds.Except(interesesActualesIds);

        // Eliminar solo los que ya no están
        var relacionesAEliminar = await trivoContexto.Set<UsuarioHabilidad>()
            .Where(ui => ui.UsuarioId == usuarioId && interesesAEliminar.Contains(ui.HabilidadId!.Value))
            .ToListAsync(cancellationToken);

        trivoContexto.Set<UsuarioHabilidad>().RemoveRange(relacionesAEliminar);

        var relacionesANuevas = interesesNuevos.Select(id => new UsuarioHabilidad
        {
            UsuarioId = usuarioId,
            HabilidadId = id
        });

        await trivoContexto.Set<UsuarioHabilidad>().AddRangeAsync(relacionesANuevas, cancellationToken);
        
        await trivoContexto.SaveChangesAsync(cancellationToken);
    }

    public async Task<ResultadoPaginado<Habilidad>> ObtenerHabilidadPaginadoAsync(int numeroPagina, int tamanoPagina, CancellationToken cancellationToken)
    {
        var total = await trivoContexto.Set<Habilidad>().AsNoTracking().CountAsync(cancellationToken);
            
        var habilidad = await trivoContexto.Set<Habilidad>().AsNoTracking()
            .Skip((numeroPagina - 1) * tamanoPagina)
            .Take(tamanoPagina)
            .ToListAsync(cancellationToken);

        return new ResultadoPaginado<Habilidad>(habilidad, total, numeroPagina, tamanoPagina);
    }

    public async Task<Habilidad> ObtenerHabilidadPorIdAsync(Guid habilidadId, CancellationToken cancellationToken)
    {
        return (await trivoContexto.Set<Habilidad>()
            .FirstOrDefaultAsync(x => x.HabilidadId == habilidadId, cancellationToken))!;
    }

    public async Task<IEnumerable<Usuario>> ObtenerUsuariosPorHabilidadesAsync(IEnumerable<Guid> habilidadId, CancellationToken cancellationToken)
    {
        return await trivoContexto.Set<Usuario>()
            .AsNoTracking()
            .Include(u => u.UsuarioHabilidades!)
                .ThenInclude(uh => uh.Habilidad)
            .Include(u => u.UsuarioInteres!)
                .ThenInclude(u => u.Interes)
            .Where(u => u.UsuarioHabilidades!.Any(uh => habilidadId.Contains(uh.HabilidadId!.Value)))
            .AsSingleQuery()
            .ToListAsync(cancellationToken);
    }
}