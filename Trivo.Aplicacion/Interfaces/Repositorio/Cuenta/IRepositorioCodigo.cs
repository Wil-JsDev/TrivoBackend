using Trivo.Dominio.Modelos;

namespace Trivo.Aplicacion.Interfaces.Repositorio.Cuenta;

public interface IRepositorioCodigo
{
    Task CreateCodigoAsync(Codigo codigo, CancellationToken cancellationToken);
    Task<Codigo> ObtenerCodigoPorIdAsync(Guid id, CancellationToken cancellationToken);
    Task <Codigo> BuscarCodigoAsync(string codigo, CancellationToken cancellationToken);
    Task EliminarTokenAsync(Codigo codigo, CancellationToken cancellationToken);
    Task<bool> ExistsCodeAsync(string codigo, CancellationToken cancellationToken);
    Task<bool> IsCodeValidAsync(string codigo,CancellationToken cancellationToken);
    Task MarkCodeAsUsedAsync(string codigo, CancellationToken cancellationToken);
    Task<bool> IsCodeUnUsedAsync(string codigo, CancellationToken cancellationToken);
}