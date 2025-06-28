using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Trivo.Aplicacion.Interfaces.Base;
using Trivo.Aplicacion.Interfaces.Repositorio;
using Trivo.Aplicacion.Paginacion;
using Trivo.Dominio.Modelos;
using Trivo.Infraestructura.Persistencia.Base;
using Trivo.Infraestructura.Persistencia.Contexto;

namespace Trivo.Infraestructura.Persistencia.Repositorio;

public class RepositorioInteres(TrivoContexto trivoContexto) : Validacion<Interes>(trivoContexto), IRepositorioInteres
{
    public async Task CrearInteresAsync(Interes interes, CancellationToken cancellationToken)
    {
        await _trivoContexto.Set<Interes>().AddAsync(interes, cancellationToken);
        await _trivoContexto.SaveChangesAsync(cancellationToken);
    }

    public async Task ActualizarInteresesDeUsuarioAsync(Guid usuarioId, List<Guid> interesIds, CancellationToken cancellationToken)
    {
        var interesesActualesIds = await _trivoContexto.Set<UsuarioInteres>()
            .Where(ui => ui.UsuarioId == usuarioId)
            .Select(ui => ui.InteresId!.Value)
            .ToListAsync(cancellationToken);

        var interesesAEliminar = interesesActualesIds.Except(interesIds);
        var interesesNuevos = interesIds.Except(interesesActualesIds);

        // Eliminar solo los que ya no están
        var relacionesAEliminar = await _trivoContexto.Set<UsuarioInteres>()
            .Where(ui => ui.UsuarioId == usuarioId && interesesAEliminar.Contains(ui.InteresId!.Value))
            .ToListAsync(cancellationToken);

        _trivoContexto.Set<UsuarioInteres>().RemoveRange(relacionesAEliminar);

        var relacionesANuevas = interesesNuevos.Select(id => new UsuarioInteres
        {
            UsuarioId = usuarioId,
            InteresId = id
        });

        await _trivoContexto.Set<UsuarioInteres>().AddRangeAsync(relacionesANuevas, cancellationToken);
        
        await _trivoContexto.SaveChangesAsync(cancellationToken);
    }
    
    public async Task<ResultadoPaginado<Interes>> ObtenerInteresesPaginadosAsync(
        int numeroPagina,
        int tamanoPagina,
        CancellationToken cancellationToken)
    {
        var total = await _trivoContexto.Set<Interes>().AsNoTracking().CountAsync(cancellationToken);
    
        var intereses = await _trivoContexto.Set<Interes>().AsNoTracking()
            .Skip((numeroPagina - 1) * tamanoPagina)
            .Take(tamanoPagina)
            .ToListAsync(cancellationToken);

        return new ResultadoPaginado<Interes>(intereses, total, numeroPagina, tamanoPagina);
    }

    public async Task<ResultadoPaginado<Interes>> ObtenerInteresesPorCategoriaPaginadosAsync(
        IEnumerable<Guid> categoriaId,
        int numeroPagina,
        int tamanoPagina,
        CancellationToken cancellationToken
        )
    {
        var query = _trivoContexto.Set<Interes>().AsNoTracking();

        IEnumerable<Guid> enumerable = categoriaId.ToList();
        if (enumerable.Any())
        {
            query = query.Where(i => enumerable.Contains(i.CategoriaId!.Value));
        }

        var total = await query.CountAsync(cancellationToken);

        var intereses = await query
            .Skip((numeroPagina - 1) * tamanoPagina)
            .Take(tamanoPagina)
            .ToListAsync(cancellationToken);

        return new ResultadoPaginado<Interes>(intereses, total, numeroPagina, tamanoPagina);
    }
    
    public async Task<IEnumerable<Usuario>> ObtenerUsuariosPorCategoriaDeInteresAsync(Guid categoriaId, CancellationToken cancellationToken)
    {
        return await _trivoContexto.Set<Usuario>()
            .AsNoTracking()
            .Include(u => u.UsuarioInteres)!
            .ThenInclude(ui => ui.Interes)
            .Include(u => u.UsuarioHabilidades)!
            .ThenInclude(uh => uh.Habilidad)
            .Where(u => u.UsuarioInteres!
                .Any(ui => ui.Interes!.CategoriaId == categoriaId))
            .AsSplitQuery()
            .ToListAsync(cancellationToken);
    }
    
    public async Task<Interes> ObtenerPorIdAsync(Guid interesId, CancellationToken cancellationToken)
    {
        return (await _trivoContexto.Set<Interes>()
            .AsNoTracking()
            .FirstOrDefaultAsync(interes => interes.Id == interesId, cancellationToken))!;
    }

    public async Task<IEnumerable<Usuario>> ObtenerUsuariosPorInteresesAsync(IEnumerable<Guid> interesesId, CancellationToken cancellationToken)
    {
        return await _trivoContexto.Set<Usuario>()
            .AsNoTracking()
            .Include(u => u.UsuarioInteres)!
            .ThenInclude(us => us.Interes)
            .Include(u => u.UsuarioHabilidades)!
                .ThenInclude(us => us.Habilidad)
            .Where(u => u.UsuarioInteres!.Any(uh => interesesId.Contains(uh.InteresId!.Value)))
            .AsSplitQuery()
            .ToListAsync(cancellationToken);
    }

    public async Task<ResultadoPaginado<Interes>> ObtenerInteresPorICategoriaIdAsync(
        IEnumerable<Guid> categoriaIds,
        int numeroPagina,
        int tamanoPagina,
        CancellationToken cancellationToken)
    {
        var query = _trivoContexto.Set<Interes>()
            .AsNoTracking()
            .Where(i => i.CategoriaId.HasValue && categoriaIds.Contains(i.CategoriaId!.Value));

        var total = await query.CountAsync(cancellationToken);

        var intereses = await query
            .Skip((numeroPagina - 1) * tamanoPagina)
            .Take(tamanoPagina)
            .Select(n => new Interes
            {
                Id = n.Id,
                Nombre = n.Nombre
            })
            .ToListAsync(cancellationToken);

        return new ResultadoPaginado<Interes>(intereses, total, numeroPagina, tamanoPagina);
    }

    public async Task<bool> NombreExisteAsync(string nombre, CancellationToken cancellationToken)
    {
        return await Validar(x => x.Nombre == nombre, cancellationToken);
    }

    public async Task<bool> NombreCategoriaExisteAsync(string nombre, Guid categoriaId, CancellationToken cancellationToken)
    {
        return await Validar(x => x.Nombre == nombre && x.CategoriaId == categoriaId, cancellationToken);
    }
}