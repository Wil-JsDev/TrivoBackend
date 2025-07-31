using Microsoft.EntityFrameworkCore;
using Trivo.Aplicacion.Interfaces.Repositorio;
using Trivo.Aplicacion.Interfaces.Repositorio.Cuenta;
using Trivo.Dominio.Modelos;
using Trivo.Infraestructura.Persistencia.Contexto;

namespace Trivo.Infraestructura.Persistencia.Repositorio.Cuenta;

public class RepositorioReclutador(TrivoContexto trivoContexto) : RepositorioGenerico<Reclutador>(trivoContexto), IRepositorioReclutador
{
    public async Task<IEnumerable<Reclutador>> FiltrarReclutadorAsync(List<Guid> habilidadesIds,
        List<Guid> interesesIds, CancellationToken cancellationToken)
    {
        return await _trivoContexto.Set<Reclutador>()
            .AsNoTracking()
            .Include(e => e.Usuario)
            .ThenInclude(u => u!.UsuarioHabilidades)
            .Include(e => e.Usuario)
            .ThenInclude(u => u!.Interes)
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

    public async Task<bool> EsUsuarioReclutadorAsync(Guid usuarioId, CancellationToken cancellationToken)
    {
        return await ValidarAsync(x => x.UsuarioId == usuarioId, cancellationToken);
    }

    public async Task<Reclutador?> ObtenerDetallesReclutadorAsync(Guid usuarioId, CancellationToken cancellationToken)
    {
        return await _trivoContexto.Set<Reclutador>()
            .AsNoTracking()
            .Include(e => e.Usuario)
                .ThenInclude(u => u!.UsuarioHabilidades)
            .Include(e => e.Usuario)
                .ThenInclude(u => u!.UsuarioInteres)
            .FirstOrDefaultAsync(e => e.UsuarioId == usuarioId, cancellationToken);
    }

    public async Task<Reclutador?> ObtenerIdAsync(Guid reclutadorId, CancellationToken cancellationToken)
    {
        return await _trivoContexto.Set<Reclutador>()
            .AsNoTracking()
            .Include(e => e.Usuario)
            .FirstOrDefaultAsync(e => e.Id == reclutadorId, cancellationToken);
    }
    
    public async Task<Reclutador?> ObtenerReclutadorPorUsuarioIdAsync(Guid usuarioId, CancellationToken cancellationToken)
    {
        return await _trivoContexto.Set<Reclutador>()
            .AsNoTracking()
            .Where(r => r.UsuarioId == usuarioId)
            .Include(r => r.Usuario)
            .AsSplitQuery()
            .FirstOrDefaultAsync(cancellationToken);
    }
}