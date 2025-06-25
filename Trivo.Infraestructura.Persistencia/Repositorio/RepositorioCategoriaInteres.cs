using Microsoft.EntityFrameworkCore;
using Trivo.Aplicacion.Interfaces.Repositorio;
using Trivo.Aplicacion.Paginacion;
using Trivo.Dominio.Modelos;
using Trivo.Infraestructura.Persistencia.Contexto;

namespace Trivo.Infraestructura.Persistencia.Repositorio;

public class RepositorioCategoriaInteres(TrivoContexto trivoContexto) : IRepositorioCategoriaInteres
{
    public async Task CrearCategoriaInteresAsync(IEnumerable<CategoriaInteres> categorias, CancellationToken cancellationToken)
    {
        await trivoContexto.Set<CategoriaInteres>().AddRangeAsync(categorias, cancellationToken);
        await trivoContexto.SaveChangesAsync(cancellationToken);
    }

    public async Task<CategoriaInteres> ObtenerPorIdAsync(Guid categoriaInteresId, CancellationToken cancellationToken)
    {
        return (await trivoContexto.Set<CategoriaInteres>()
            .AsNoTracking()
            .FirstOrDefaultAsync(ci => ci.CategoriaId == categoriaInteresId, cancellationToken))!;
    }

    public async Task<ResultadoPaginado<CategoriaInteres>> ObtenerCategoriaInteresPaginadoAsync(int numeroPagina, int tamanoPagina, CancellationToken cancellationToken)
    {
        var total = await trivoContexto.Set<Habilidad>().AsNoTracking().CountAsync(cancellationToken);
            
        var categoriaInteres = await trivoContexto.Set<CategoriaInteres>().AsNoTracking()
            .Skip((numeroPagina - 1) * tamanoPagina)
            .Take(tamanoPagina)
            .ToListAsync(cancellationToken);

        return new ResultadoPaginado<CategoriaInteres>(categoriaInteres, total, numeroPagina, tamanoPagina);
    }

    public async Task ActualizarCategoriaInteresAsync(CategoriaInteres categoriaInteres, CancellationToken cancellationToken)
    {
        trivoContexto.Set<CategoriaInteres>().Attach(categoriaInteres);
        trivoContexto.Entry(categoriaInteres).State = EntityState.Modified;
        await trivoContexto.SaveChangesAsync(cancellationToken);
    }
}