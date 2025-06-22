using Trivo.Dominio.Modelos;

namespace Trivo.Aplicacion.Interfaces.Repositorio.Cuenta;

public interface IRepositorioCodigo
{
    Task CrearCodigoAsync(Codigo codigo, CancellationToken cancellationToken);
    Task<Codigo> ObtenerCodigoPorIdAsync(Guid id, CancellationToken cancellationToken);
    Task <Codigo> BuscarCodigoAsync(string codigo, CancellationToken cancellationToken);
    Task EliminarCodigoAsync(Codigo codigo, CancellationToken cancellationToken);
    Task<bool> ExisteElCodigoAsync(string codigo, CancellationToken cancellationToken);
    Task<bool> ElCodigoEsValidoAsync(string codigo,CancellationToken cancellationToken);
    Task MarcarCodigoComoUsado(string codigo, CancellationToken cancellationToken);
    Task<bool> CodigoNoUsadoAsync(string codigo, CancellationToken cancellationToken);
}