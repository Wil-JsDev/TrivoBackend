using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Trivo.Aplicacion.Interfaces.Base;
using Trivo.Infraestructura.Persistencia.Contexto;

namespace Trivo.Infraestructura.Persistencia.Base;

public class Validacion<TEntidad>(TrivoContexto trivoContexto) : IValidacion
    <TEntidad> where TEntidad : class
{
    protected readonly TrivoContexto _trivoContexto = trivoContexto;
    
    public async Task<bool> Validar(Expression<Func<TEntidad, bool>> entidadExpresion, CancellationToken cancellationToken)
    {
        return await _trivoContexto.Set<TEntidad>()
            .AsNoTracking()
            .AnyAsync(entidadExpresion, cancellationToken);
    }
}