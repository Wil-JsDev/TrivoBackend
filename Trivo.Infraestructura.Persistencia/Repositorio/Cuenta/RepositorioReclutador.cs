using Microsoft.EntityFrameworkCore;
using Trivo.Aplicacion.Interfaces.Repositorio;
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
}