using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Trivo.Aplicacion.Interfaces.Repositorio;
using Trivo.Aplicacion.Paginacion;
using Trivo.Dominio.Modelos;
using Trivo.Infraestructura.Persistencia.Base;
using Trivo.Infraestructura.Persistencia.Contexto;

namespace Trivo.Infraestructura.Persistencia.Repositorio;

public class RepositorioCategoriaInteres(TrivoContexto trivoContexto) : Validacion<CategoriaInteres>(trivoContexto),IRepositorioCategoriaInteres
{
    public async Task CrearCategoriaInteresAsync(CategoriaInteres categorias, CancellationToken cancellationToken)
    {
        await _trivoContexto.Set<CategoriaInteres>().AddAsync(categorias, cancellationToken);
        await _trivoContexto.SaveChangesAsync(cancellationToken);
    }

    public async Task<CategoriaInteres> ObtenerPorIdAsync(Guid categoriaInteresId, CancellationToken cancellationToken)
    {
        return (await _trivoContexto.Set<CategoriaInteres>()
            .AsNoTracking()
            .FirstOrDefaultAsync(ci => ci.CategoriaId == categoriaInteresId, cancellationToken))!;
    }

    public async Task<ResultadoPaginado<CategoriaInteres>> ObtenerCategoriaInteresPaginadoAsync(int numeroPagina, int tamanoPagina, CancellationToken cancellationToken)
    {
        var total = await _trivoContexto.Set<CategoriaInteres>().AsNoTracking().CountAsync(cancellationToken);
            
        var categoriaInteres = await _trivoContexto.Set<CategoriaInteres>().AsNoTracking()
            .Skip((numeroPagina - 1) * tamanoPagina)
            .Take(tamanoPagina)
            .ToListAsync(cancellationToken);

        return new ResultadoPaginado<CategoriaInteres>(categoriaInteres, total, numeroPagina, tamanoPagina);
    }

    public async Task ActualizarCategoriaInteresAsync(CategoriaInteres categoriaInteres, CancellationToken cancellationToken)
    {
        _trivoContexto.Set<CategoriaInteres>().Attach(categoriaInteres);
        _trivoContexto.Entry(categoriaInteres).State = EntityState.Modified;
        await _trivoContexto.SaveChangesAsync(cancellationToken);
    }

    public async Task<bool> NombreExisteAsync(string nombre, CancellationToken cancellationToken)
    {
        return await Validar(x => x.Nombre == nombre, cancellationToken);
    }
    
}