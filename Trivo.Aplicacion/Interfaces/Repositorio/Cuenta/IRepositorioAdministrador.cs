using Trivo.Aplicacion.Paginacion;
using Trivo.Dominio.Modelos;

namespace Trivo.Aplicacion.Interfaces.Repositorio.Cuenta;

public interface IRepositorioAdministrador : IRepositorioGenerico<Administrador>
{
    Task BanearUsuarioAsync(Guid usuarioId, CancellationToken cancellationToken);

    Task DesbanearUsuarioAsync(Guid usuarioId, CancellationToken cancellationToken);

    Task<ResultadoPaginado<Reporte>> ObtenerPaginadoUltimosReportes(
        int numeroPagina,
        int tamanoPagina,
        CancellationToken cancellationToken);

    Task<IEnumerable<Usuario>> ObtenerUltimos10UsuariosBaneadosAsync(CancellationToken cancellationToken);
    
    Task<bool> NombreUsuarioEnUso(string nombreUsuario, Guid usuarioId, CancellationToken cancellationToken);

    Task<Administrador> BuscarPorEmailAsync(string email, CancellationToken cancellationToken);

    Task<bool> EmailEnUsoAsync(string email, Guid excluirUsuarioId, CancellationToken cancellationToken);

    Task<bool> ExisteEmailAsync(string email, CancellationToken cancellationToken);
    
    Task<bool> ExisteNombreUsuarioAsync(string nombreUsuario, CancellationToken cancellationToken);

    Task ActualizarContrasenaAsync(Administrador admin, string nuevaContrasena);
    
    Task<ResultadoPaginado<Usuario>> ObtenerPaginadoUltimosUsuariosAsync(
        int numeroPagina,
        int tamanoPagina,
        CancellationToken cancellationToken);
    
    Task<ResultadoPaginado<Emparejamiento>> ObtenerPaginadoUltimosEmparejamientosAsync(
        int numeroPagina,
        int tamanoPagina,
        CancellationToken cancellationToken);
    
    // Dto: id y nombre de ambos roles, junto del emparejamiento dto en la paginacion de ultimos emparejamientos
    // Banear usuarios y desbanear 
    // Eliminar notificaciones
    
    Task<int> ContarEmparejamientosCompletadosAsync(CancellationToken cancellationToken);

    Task<int> ContarUsuariosActivosAsync(CancellationToken cancellationToken);
}