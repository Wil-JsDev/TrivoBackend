namespace Trivo.Aplicacion.Interfaces.Servicios;

public interface IObtenerReclutadorIdServicio
{
    Task<Guid?> ObtenerReclutadorIdAsync(Guid usuarioId, CancellationToken cancellationToken);
}