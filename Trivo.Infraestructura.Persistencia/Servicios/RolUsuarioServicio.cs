using Microsoft.EntityFrameworkCore;
using Trivo.Aplicacion.Interfaces.Servicios;
using Trivo.Dominio.Enum;
using Trivo.Dominio.Modelos;
using Trivo.Infraestructura.Persistencia.Contexto;

namespace Trivo.Infraestructura.Persistencia.Servicios;

public class RolUsuarioServicio(
    TrivoContexto trivoContexto
) : IRolUsuarioServicio
{
    public async Task<IList<Roles>> ObtenerRolesAsync(Guid usuarioId, CancellationToken cancellationToken)
    {
        var roles = new List<Roles>();

        var esReclutador = await trivoContexto.Set<Reclutador>()
            .AsNoTracking()
            .AnyAsync(r => r.UsuarioId == usuarioId, cancellationToken);
        
        if (esReclutador)
            roles.Add(Roles.Reclutador);

        var esExperto = await trivoContexto.Set<Experto>()
            .AsNoTracking()
            .AnyAsync(e => e.UsuarioId == usuarioId, cancellationToken);
        
        if (esExperto)
            roles.Add(Roles.Experto);

        // Obtener el correo del usuario para verificar si es Administrador
        var correo = await trivoContexto.Set<Usuario>()
            .AsNoTracking()
            .Where(u => u.Id == usuarioId)
            .Select(u => u.Email)
            .FirstOrDefaultAsync(cancellationToken);

        if (string.IsNullOrWhiteSpace(correo)) return roles;
        
        var esAdmin = await trivoContexto.Set<Administrador>()
            .AsNoTracking()
            .AnyAsync(a => a.Email == correo, cancellationToken);

        if (esAdmin)
            roles.Add(Roles.Administrador);


        return roles;
    }
}