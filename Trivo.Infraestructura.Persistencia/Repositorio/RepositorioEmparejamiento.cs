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
                .ThenInclude(e => e!.Usuario)
                    .ThenInclude(us => us!.UsuarioHabilidades)!
                        .ThenInclude(uh => uh.Habilidad)
            .Include(e => e.Experto)
                .ThenInclude(e => e!.Usuario)
                    .ThenInclude(us => us!.UsuarioInteres)!
                        .ThenInclude(ui => ui.Interes)
            .Include(e => e.Reclutador)
                .ThenInclude(r => r!.Usuario)
            .AsSplitQuery()
            .Where(e => e.Experto!.UsuarioId == usuarioId)
            .Where(x => x.EmparejamientoEstado == nameof(EmparejamientoEstado.Pendiente))
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<Emparejamiento>> ObtenerEmparejamientosComoReclutadorAsync(Guid usuarioId, CancellationToken cancellationToken)
    {
        return await _trivoContexto.Set<Emparejamiento>()
            .AsNoTracking()
            .Include(e => e.Reclutador)
                .ThenInclude(r => r!.Usuario)
                    .ThenInclude(u => u!.UsuarioHabilidades)!
            .ThenInclude(uh => uh.Habilidad)
            .Include(e => e.Reclutador)
                    .ThenInclude(r => r!.Usuario)
                        .ThenInclude(u => u!.UsuarioInteres)!
                            .ThenInclude(ui => ui.Interes)
            .Include(e => e.Experto)
                .ThenInclude(ex => ex!.Usuario)
            .AsSplitQuery()
            .Where(x => x.Reclutador!.UsuarioId == usuarioId)
            .Where(x => x.EmparejamientoEstado == nameof(EmparejamientoEstado.Pendiente))
            .ToListAsync(cancellationToken);
    }

    public async Task<Emparejamiento?> ObtenerPorIdAsync(Guid emparejamientoId, CancellationToken cancellationToken)
    {
        return await _trivoContexto.Set<Emparejamiento>()
            .AsNoTracking()
            .Include(x => x.Experto)
                .ThenInclude(e => e!.Usuario)
            .Include(x => x.Reclutador)
            .ThenInclude(e => e!.Usuario)
            .AsSplitQuery()
            .FirstOrDefaultAsync(x => x.Id == emparejamientoId, cancellationToken);
    }
}