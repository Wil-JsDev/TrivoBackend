namespace Trivo.Aplicacion.Interfaces.Servicios;

public interface IObtenerExpertoIdServicio
{
    Task<Guid?> ObtenerExpertoIdAsync(Guid usuarioId, CancellationToken cancellationToken);
}