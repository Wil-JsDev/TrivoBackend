using Trivo.Dominio.Modelos;

namespace Trivo.Aplicacion.Interfaces.Repositorio.Cuenta;

public interface IRepositorioAdministrador : IRepositorioGenerico<Administrador>
{
    Task BanearUsuarioAsync(Guid usuarioId, CancellationToken cancellationToken);

    Task DesbanearUsuarioAsync(Guid usuarioId, CancellationToken cancellationToken);
    
    Task<IEnumerable<Usuario>> ObtenerUsuariosBaneadosAsync(CancellationToken cancellationToken);
    
}