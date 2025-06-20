using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Trivo.Aplicacion.Interfaces.Repositorio;
using Trivo.Aplicacion.Paginacion;
using Trivo.Infraestructura.Persistencia.Contexto;

namespace Trivo.Infraestructura.Persistencia.Repositorio;

public class RepositorioGenerico<TEntidad>: IRepositorioGenerico<TEntidad> where TEntidad : class
{
    protected readonly TrivoContexto _trivoContexto;
    

    public async Task<TEntidad> ObtenerByIdAsync(Guid id, CancellationToken cancellationToken) =>
    await _trivoContexto.Set<TEntidad>().FindAsync(id, cancellationToken);

    public async Task<ResultadoPaginado<TEntidad>> ObtenerPaginadoAsync(int numeroPagina, int tamanioPagina, CancellationToken cancellationToken)
    {
        var total = await _trivoContexto.Set<TEntidad>().AsNoTracking().CountAsync(cancellationToken);
        
        var entidad = await _trivoContexto.Set<TEntidad>().AsNoTracking()
            .Skip((numeroPagina - 1) * tamanioPagina)
            .Take(tamanioPagina)
            .ToListAsync(cancellationToken);
        
        return new ResultadoPaginado<TEntidad>(entidad, total, numeroPagina, tamanioPagina);
    }

    public async Task CrearAsync(TEntidad entidad, CancellationToken cancellationToken)
    {
        await _trivoContexto.Set<TEntidad>().AddAsync(entidad, cancellationToken);
        await GuardarAsync(cancellationToken);
    }

    public async Task ActualizarAsync(TEntidad entidad, CancellationToken cancellationToken)
    {
        _trivoContexto.Set<TEntidad>().Attach(entidad);
        _trivoContexto.Entry(entidad).State = EntityState.Modified;
        await GuardarAsync(cancellationToken);
    }

    public async Task EliminarAsync(TEntidad entidad, CancellationToken cancellationToken)
    {
        _trivoContexto.Remove(entidad);
        await GuardarAsync(cancellationToken);
    }

    public async Task<bool> ValidarAsync(Expression<Func<TEntidad, bool>> predicate, CancellationToken cancellationToken) => 
        await _trivoContexto.Set<TEntidad>()
            .AsNoTracking()
            .AnyAsync(predicate, cancellationToken);
    

    public async Task GuardarAsync(CancellationToken cancellationToken) =>
    await _trivoContexto.SaveChangesAsync(cancellationToken);
}