using Trivo.Dominio.Modelos;

namespace Trivo.Aplicacion.Interfaces.Repositorio.Cuenta;

public interface IRepositorioAdministrador : IRepositorioGenerico<Administrador>
{
    Task BanearUsuarioAsync(Guid usuarioId, CancellationToken cancellationToken);

    Task DesbanearUsuarioAsync(Guid usuarioId, CancellationToken cancellationToken);
    
    Task<IEnumerable<Usuario>> ObtenerUsuariosBaneadosAsync(CancellationToken cancellationToken);

    Task<bool> NombreUsuarioEnUso(string nombreUsuario, Guid usuarioId, CancellationToken cancellationToken);

    Task<Usuario> BuscarPorEmailAsync(string email, CancellationToken cancellationToken);

    Task<bool> EmailEnUsoAsync(string email, Guid excluirUsuarioId, CancellationToken cancellationToken);

    Task<bool> ExisteEmailAsync(string email, CancellationToken cancellationToken);
    
    Task<bool> ExisteNombreUsuarioAsync(string nombreUsuario, CancellationToken cancellationToken);

    Task ActualizarContrasenaAsync(Administrador admin, string nuevaContrasena);
}