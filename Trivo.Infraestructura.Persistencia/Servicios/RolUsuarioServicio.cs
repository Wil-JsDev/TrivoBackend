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
    public async Task<IList<Roles>> ObtenerRolesAsync(Usuario usuario, CancellationToken cancellationToken)
    {
        var roles = new List<Roles>();

        if (usuario.Reclutadores?.Any() is true)
            roles.Add(Roles.Reclutador);

        if (usuario.Expertos?.Any() is true)
            roles.Add(Roles.Experto);

        var esAdmin = await trivoContexto.Set<Administrador>()
            .AsNoTracking()
            .AnyAsync(a => a.Email == usuario.Email, cancellationToken);

        if (esAdmin)
            roles.Add(Roles.Administrador);

        return roles;
    }
}