using Microsoft.EntityFrameworkCore;
using Trivo.Aplicacion.Interfaces.Repositorio.Cuenta;
using Trivo.Dominio.Modelos;
using Trivo.Infraestructura.Persistencia.Contexto;

namespace Trivo.Infraestructura.Persistencia.Repositorio.Cuenta;

public class RepositorioCodigo(TrivoContexto contexto): RepositorioGenerico<Codigo>(contexto), IRepositorioCodigo
{
    public async Task CreateCodigoAsync(Codigo codigo, CancellationToken cancellationToken)
    {
        await _trivoContexto.Set<Codigo>().AddAsync(codigo, cancellationToken);
        await _trivoContexto.SaveChangesAsync(cancellationToken);
    }

    public async Task<Codigo> ObtenerCodigoPorIdAsync(Guid id, CancellationToken cancellationToken) =>
        await _trivoContexto.Set<Codigo>()
            .AsNoTracking()
            .FirstOrDefaultAsync(c => c.CodigoId == id,cancellationToken);
    

    public async Task<Codigo> BuscarCodigoAsync(string codigo, CancellationToken cancellationToken) =>
        await _trivoContexto.Set<Codigo>()
            .AsNoTracking()
            .FirstOrDefaultAsync(c => c.Valor == codigo, cancellationToken);

    public async Task EliminarTokenAsync(Codigo codigo, CancellationToken cancellationToken)
    {
        _trivoContexto.Set<Codigo>().Remove(codigo);
        await _trivoContexto.SaveChangesAsync(cancellationToken);
    }

    public async Task<bool> ExistsCodeAsync(string codigo, CancellationToken cancellationToken) =>
        await _trivoContexto.Set<Codigo>()
            .AsNoTracking()
            .AnyAsync(c => c.Valor == codigo, cancellationToken);

    public async Task<bool> IsCodeValidAsync(string codigo, CancellationToken cancellationToken) =>
        await ValidarAsync(c => c.Valor == codigo, cancellationToken);

    public async Task MarkCodeAsUsedAsync(string codigo, CancellationToken cancellationToken)
    {
       var usuarioCodigo = await _trivoContexto.Set<Codigo>()
           .FirstOrDefaultAsync(c => c.Valor == codigo, cancellationToken);

       if (usuarioCodigo != null)
       {
           usuarioCodigo.Usado = true;
           await _trivoContexto.SaveChangesAsync(cancellationToken);
       }
    }

    public async Task<bool> IsCodeUnUsedAsync(string codigo, CancellationToken cancellationToken) =>
        await _trivoContexto.Set<Codigo>()
            .AsNoTracking()
            .AnyAsync(c => c.Valor == codigo && c.Usado == false, cancellationToken);

}