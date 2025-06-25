using Microsoft.EntityFrameworkCore;
using Trivo.Aplicacion.Interfaces.Repositorio;
using Trivo.Dominio.Modelos;
using Trivo.Infraestructura.Persistencia.Contexto;

namespace Trivo.Infraestructura.Persistencia.Repositorio;

public class RepositorioUsuarioInteres(TrivoContexto trivoContexto) : IRepositorioUsuarioInteres
{
    public async Task CrearUsuarioInteresAsync(UsuarioInteres usuarioInteres, CancellationToken cancellationToken)
    {
        await trivoContexto.Set<UsuarioInteres>().AddAsync(usuarioInteres, cancellationToken);
        await trivoContexto.SaveChangesAsync(cancellationToken);
    }

    public async Task<List<UsuarioInteres>> AsociarInteresesAlUsuarioAsync(Guid usuarioId, List<Guid> interesesIds, CancellationToken cancellationToken)
    {
        // Obtener los intereses ya asociados por ese usuario
        var interesesExistentes = await trivoContexto.Set<UsuarioInteres>()
            .AsNoTracking()
            .Where(ui => ui.UsuarioId == usuarioId && interesesIds.Contains(ui.InteresId!.Value))
            .Select(ui => ui.InteresId!.Value)
            .ToListAsync(cancellationToken);

        var nuevosInteresIds = interesesIds.Except(interesesExistentes).ToList();

        if (!nuevosInteresIds.Any())
            return new List<UsuarioInteres>();

        // Nuevas relaciones
        var nuevasAsociaciones = nuevosInteresIds.Select(interesId => new UsuarioInteres
        {
            UsuarioId = usuarioId,
            InteresId = interesId
        }).ToList();

        await trivoContexto.Set<UsuarioInteres>().AddRangeAsync(nuevasAsociaciones, cancellationToken);

        await trivoContexto.SaveChangesAsync(cancellationToken);
            
        return nuevasAsociaciones;
    }
}