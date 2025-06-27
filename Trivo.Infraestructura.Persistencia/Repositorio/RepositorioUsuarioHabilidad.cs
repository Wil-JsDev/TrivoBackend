using Microsoft.EntityFrameworkCore;
using Trivo.Aplicacion.Interfaces.Repositorio;
using Trivo.Dominio.Enum;
using Trivo.Dominio.Modelos;
using Trivo.Infraestructura.Persistencia.Contexto;

namespace Trivo.Infraestructura.Persistencia.Repositorio;

public class RepositorioUsuarioHabilidad(TrivoContexto trivoContexto) : IRepositorioUsuarioHabilidad
{
    
    public async Task CrearHabilidadesUsuarioAsync(UsuarioHabilidad usuarioHabilidad, CancellationToken cancellationToken)
    {
        await trivoContexto.Set<UsuarioHabilidad>().AddAsync(usuarioHabilidad, cancellationToken);
        await trivoContexto.SaveChangesAsync(cancellationToken);
    }

    public async Task<List<UsuarioHabilidad>> AsociarHabilidadesAlUsuarioAsync(Guid usuarioId, List<Guid> habilidadIds, CancellationToken cancellationToken)
    {
        // Obtener loas habilidades ya asociadas por un usuario
        var habilidadesExistentes = await trivoContexto.Set<UsuarioHabilidad>()
            .AsNoTracking()
            .Where(uh => uh.UsuarioId == usuarioId && habilidadIds.Contains(uh.HabilidadId!.Value))
            .Select(uh => uh.HabilidadId!.Value)
            .ToListAsync(cancellationToken);
        
        var habilidadesNuevasIds = habilidadIds.Except(habilidadesExistentes).ToList();

        if (!habilidadesExistentes.Any())
            return [];
        
        // Nuevas relaciones
        var nuevasAsociaciones = habilidadesNuevasIds.Select(habilidadId => new UsuarioHabilidad()
        {
            UsuarioId = usuarioId,
            HabilidadId = habilidadId
        }).ToList();

        await trivoContexto.Set<UsuarioHabilidad>().AddRangeAsync(nuevasAsociaciones,cancellationToken);
        
        await trivoContexto.SaveChangesAsync(cancellationToken);
        
        return nuevasAsociaciones;
    }

    public async Task ActualizarNivelHabilidadUsuarioAsync(Guid usuarioId, Guid habilidadId, Nivel nivel,
        CancellationToken cancellationToken)
    {
        var usuarioHabilidad = await trivoContexto.Set<UsuarioHabilidad>()
            .FirstOrDefaultAsync(uh => uh.UsuarioId == usuarioId && uh.HabilidadId == habilidadId, cancellationToken);

        usuarioHabilidad!.Nivel = nivel.ToString();
        
        await trivoContexto.SaveChangesAsync(cancellationToken);
    }
}