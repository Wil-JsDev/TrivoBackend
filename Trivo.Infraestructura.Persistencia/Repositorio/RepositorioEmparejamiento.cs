using Microsoft.EntityFrameworkCore;
using Trivo.Aplicacion.Interfaces.Repositorio;
using Trivo.Dominio.Enum;
using Trivo.Dominio.Modelos;
using Trivo.Infraestructura.Persistencia.Contexto;

namespace Trivo.Infraestructura.Persistencia.Repositorio;

public class RepositorioEmparejamiento(TrivoContexto trivoContexto) : RepositorioGenerico<Emparejamiento>(trivoContexto), IRepositorioEmparejamiento
{
    public async Task<Emparejamiento?> ObtenerEmparejamientoAsync(Guid expertoId, Guid reclutadorId, CancellationToken cancellationToken)
    {
        return await _trivoContexto.Set<Emparejamiento>()
            .AsNoTracking()
            .FirstOrDefaultAsync(e => e.ExpertoId == expertoId && e.ReclutadorId == reclutadorId, cancellationToken);
    }

    public async Task<bool> ExisteEmparejamientoAsync(Guid expertoId, Guid reclutadorId, CancellationToken cancellationToken)
    {
        return await ValidarAsync(x => x.ExpertoId == expertoId && x.ReclutadorId == reclutadorId, cancellationToken);
    }

    public async Task<IEnumerable<Emparejamiento>> ObtenerEmparejamientosComoExpertoAsync(Guid usuarioId, CancellationToken cancellationToken)
    {
        return await _trivoContexto.Set<Emparejamiento>()
            .AsNoTracking()
            .Include(e => e.Experto)
                .ThenInclude(x => x!.Usuario)   
            .Where(e => e.Reclutador!.UsuarioId == usuarioId)
            .Where(x => x.EmparejamientoEstado == nameof(EmparejamientoEstado.Pendiente))
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<Emparejamiento>> ObtenerEmparejamientosComoReclutadorAsync(Guid usuarioId, CancellationToken cancellationToken)
    {
        return await _trivoContexto.Set<Emparejamiento>()
            .AsNoTracking()
            .Include(e => e.Reclutador)
                .ThenInclude(x => x!.Usuario)
            .Where(x => x.Experto!.UsuarioId == usuarioId)
            .Where(x => x.EmparejamientoEstado == nameof(EmparejamientoEstado.Pendiente))
            .ToListAsync(cancellationToken);
    }
    
}