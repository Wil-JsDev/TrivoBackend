using Trivo.Dominio.Enum;
using Trivo.Dominio.Modelos;

namespace Trivo.Aplicacion.Interfaces.Servicios;

public interface IRolUsuarioServicio
{
    Task<IList<Roles>> ObtenerRolesAsync(Usuario usuario, CancellationToken cancellationToken);
}