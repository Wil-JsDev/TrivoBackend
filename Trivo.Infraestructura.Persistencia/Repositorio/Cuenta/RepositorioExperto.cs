using Microsoft.EntityFrameworkCore;
using Trivo.Aplicacion.Interfaces.Repositorio;
using Trivo.Aplicacion.Interfaces.Repositorio.Cuenta;
using Trivo.Dominio.Modelos;
using Trivo.Infraestructura.Persistencia.Contexto;

namespace Trivo.Infraestructura.Persistencia.Repositorio.Cuenta;

public class RepositorioExperto(TrivoContexto trivoContexto) : RepositorioGenerico<Experto>(trivoContexto), IRepositorioExperto
{
    public async Task<IEnumerable<Experto>> FiltrarExpertoAsync(
        List<Guid> habilidadesIds, 
        List<Guid> interesesIds, 
        CancellationToken cancellationToken
    )
    {
        return await _trivoContexto.Set<Experto>()
            .AsNoTracking()
            .Include(e => e.Usuario)
                .ThenInclude(u => u!.UsuarioHabilidades)
            .Include(e => e.Usuario)
                .ThenInclude(u => u!.Interes)
            .AsSplitQuery()
            .Where(e => 
                e.Usuario != null && 
                (habilidadesIds.Count == 0 || 
                 e.Usuario.UsuarioHabilidades!
                     .Any(uh => habilidadesIds.Contains(uh.HabilidadId!.Value))) && 
                (interesesIds.Count == 0 || 
                 e.Usuario.Interes!
                     .Any(i => interesesIds.Contains(i.Id ?? Guid.Empty))))
            .ToListAsync(cancellationToken);
    }

    public async Task<bool> EsUsuarioExpertoAsync(Guid usuarioId, CancellationToken cancellationToken)
    {
        return await ValidarAsync(x => x.UsuarioId == usuarioId, cancellationToken);
    }

    public async Task<Experto?> ObtenerDetallesExpertoAsync(Guid usuarioId, CancellationToken cancellationToken)
    {
        return await _trivoContexto.Set<Experto>()
            .AsNoTracking()
            .Include(e => e.Usuario)
                .ThenInclude(u => u!.UsuarioHabilidades)
            .Include(e => e.Usuario)
                .ThenInclude(u => u!.UsuarioInteres)
            .FirstOrDefaultAsync(e => e.UsuarioId == usuarioId, cancellationToken);
    }

    public async Task<Experto?> ObtenerIdAsync(Guid expertoId, CancellationToken cancellationToken)
    {
       return await _trivoContexto.Set<Experto>()
            .AsNoTracking()
            .Include(e => e.Usuario)
            .FirstOrDefaultAsync(e => e.Id == expertoId, cancellationToken);
    }
    public async Task<Experto?> ObtenerExpertoPorUsuarioIdAsync(Guid usuarioId, CancellationToken cancellationToken)
    {
        return await _trivoContexto.Set<Experto>()
            .AsNoTracking()
            .Where(e => e.UsuarioId == usuarioId)
            .Include(e => e.Usuario)
            .ThenInclude(u => u!.UsuarioHabilidades)!
            .ThenInclude(u => u!.Habilidad)
            .Include(e => e.Usuario)
            .ThenInclude(u => u!.UsuarioInteres)!
            .ThenInclude(u => u!.Interes)
            .AsSplitQuery()
            .FirstOrDefaultAsync(cancellationToken);
    }
}