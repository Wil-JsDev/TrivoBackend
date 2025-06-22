using Trivo.Dominio.Enum;
using Trivo.Dominio.Modelos;

namespace Trivo.Aplicacion.Interfaces.Repositorio.Cuenta;

public interface IRepositorioUsuario: IRepositorioGenerico<Usuario>
{
    Task<bool> CuentaConfirmadaAsync(Guid id, CancellationToken cancellationToken);
    Task<bool> NombreUsuarioEnUso(string nombreUsuario, Guid usuarioId, CancellationToken cancellationToken);
    Task<EstadoUsuario?> ObtenerEstadoUsuarioAsync(Guid usuarioId, CancellationToken cancellationToken);
    Task<Usuario> BuscarPorEmailUsuarioAsync(string email, CancellationToken cancellationToken);
    Task<Usuario> BuscarPorNombreUsuarioAsync(string nombreUsuario, CancellationToken cancellationToken);
    Task<IEnumerable<Usuario>> FiltrarPorHabilidadesAsync(List<Guid> habilidadesIds, CancellationToken cancellationToken);
    Task<IEnumerable<Usuario>> FiltrarPorInteresesAsync(List<Guid> interesesIds, CancellationToken cancellationToken);
    Task<List<Guid>> ObtenerHabilidadesUsuarioAsync(Guid usuarioId, CancellationToken cancellationToken);
    Task<List<Guid>> ObtenerInteresesUsuarioAsync(Guid usuarioId, CancellationToken cancellationToken);



}