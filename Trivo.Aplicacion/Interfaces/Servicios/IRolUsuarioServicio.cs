using Trivo.Dominio.Enum;
using Trivo.Dominio.Modelos;

namespace Trivo.Aplicacion.Interfaces.Servicios;

public interface IRolUsuarioServicio
{
    Task<IList<Roles>> ObtenerRolesAsync(Guid usuarioId, CancellationToken cancellationToken);
}