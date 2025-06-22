using Microsoft.EntityFrameworkCore;
using Trivo.Aplicacion.Interfaces.Repositorio.Cuenta;
using Trivo.Dominio.Modelos;
using Trivo.Infraestructura.Persistencia.Contexto;

namespace Trivo.Infraestructura.Persistencia.Repositorio.Cuenta;

public class RepositorioCodigo(TrivoContexto contexto): RepositorioGenerico<Codigo>(contexto), IRepositorioCodigo
{
    public async Task CrearCodigoAsync(Codigo codigo, CancellationToken cancellationToken)
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

    public async Task EliminarCodigoAsync(Codigo codigo, CancellationToken cancellationToken)
    {
        _trivoContexto.Set<Codigo>().Remove(codigo);
        await _trivoContexto.SaveChangesAsync(cancellationToken);
    }

    public async Task<bool> ExisteElCodigoAsync(string codigo, CancellationToken cancellationToken) =>
        await ValidarAsync(c => c.Valor == codigo, cancellationToken);
    public async Task<bool> ElCodigoEsValidoAsync(string codigo, CancellationToken cancellationToken) =>
        await ValidarAsync(c => c.Valor == codigo &&
                                c.Expiracion > DateTime.UtcNow &&
                                !c.Usado.Value, cancellationToken);

    public async Task MarcarCodigoComoUsado(string codigo, CancellationToken cancellationToken)
    {
        var usuarioCodigo = await _trivoContexto.Set<Codigo>()
            .FirstOrDefaultAsync(c => c.Valor == codigo, cancellationToken);

        if (usuarioCodigo != null)
        {
            usuarioCodigo.Usado = true;
            await _trivoContexto.SaveChangesAsync(cancellationToken);
        }    
    }

    public async Task<bool> CodigoNoUsadoAsync(string codigo, CancellationToken cancellationToken) =>
        await ValidarAsync(c => c.Valor == codigo && c.Usado.Value, cancellationToken);

}