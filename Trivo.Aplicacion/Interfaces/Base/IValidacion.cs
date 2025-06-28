using System.Linq.Expressions;

namespace Trivo.Aplicacion.Interfaces.Base;

public interface IValidacion<TEntidad> where TEntidad : class
{
    Task<bool> Validar (Expression<Func<TEntidad, bool>> entidadExpresion, CancellationToken cancellationToken);
}