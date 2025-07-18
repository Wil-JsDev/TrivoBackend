using Trivo.Dominio.Enum;
using Trivo.Dominio.Modelos;

namespace Trivo.Aplicacion.Interfaces.Repositorio.Cuenta;

public interface IRepositorioUsuario: IRepositorioGenerico<Usuario>
{
    /// <summary>
    /// Verifica si la cuenta de un usuario esta confirmada.
    /// </summary>
    /// <param name="id">Id del usuario.</param>
    /// <param name="cancellationToken">Token para cancelar la operacion.</param>
    /// <returns>True si la cuenta esta confirmada, false en caso contrario.</returns>
    Task<bool> CuentaConfirmadaAsync(Guid id, CancellationToken cancellationToken);

    /// <summary>
    /// Verifica si un nombre de usuario esta en uso por otro usuario distinto al especificado.
    /// </summary>
    /// <param name="nombreUsuario">Nombre de usuario a verificar.</param>
    /// <param name="usuarioId">Id del usuario que se excluye de la verificacion.</param>
    /// <param name="cancellationToken">Token para cancelar la operacion.</param>
    /// <returns>True si el nombre de usuario esta en uso, false en caso contrario.</returns>
    Task<bool> NombreUsuarioEnUso(string nombreUsuario, Guid usuarioId, CancellationToken cancellationToken);

    /// <summary>
    /// Obtiene el estado de un usuario dado su Id.
    /// </summary>
    /// <param name="usuarioId">Id del usuario.</param>
    /// <param name="cancellationToken">Token para cancelar la operacion.</param>
    /// <returns>Estado del usuario o null si no existe.</returns>
    Task<string?> ObtenerEstadoUsuarioAsync(Guid usuarioId, CancellationToken cancellationToken);

    /// <summary>
    /// Busca un usuario por su email.
    /// </summary>
    /// <param name="email">Email del usuario.</param>
    /// <param name="cancellationToken">Token para cancelar la operacion.</param>
    /// <returns>Usuario encontrado o null si no existe.</returns>
    Task<Usuario> BuscarPorEmailUsuarioAsync(string email, CancellationToken cancellationToken);

    /// <summary>
    /// Busca un usuario por su nombre de usuario.
    /// </summary>
    /// <param name="nombreUsuario">Nombre de usuario a buscar.</param>
    /// <param name="cancellationToken">Token para cancelar la operacion.</param>
    /// <returns>Usuario encontrado o null si no existe.</returns>
    Task<Usuario> BuscarPorNombreUsuarioAsync(string nombreUsuario, CancellationToken cancellationToken);

    /// <summary>
    /// Filtra usuarios que tengan las habilidades indicadas.
    /// </summary>
    /// <param name="habilidadesIds">Lista de Ids de habilidades.</param>
    /// <param name="cancellationToken">Token para cancelar la operacion.</param>
    /// <returns>Lista de usuarios que cumplen el filtro.</returns>
    Task<IEnumerable<Usuario>> FiltrarPorHabilidadesAsync(List<Guid> habilidadesIds, CancellationToken cancellationToken);

    /// <summary>
    /// Filtra usuarios que tengan los intereses indicados.
    /// </summary>
    /// <param name="interesesIds">Lista de Ids de intereses.</param>
    /// <param name="cancellationToken">Token para cancelar la operacion.</param>
    /// <returns>Lista de usuarios que cumplen el filtro.</returns>
    Task<IEnumerable<Usuario>> FiltrarPorInteresesAsync(List<Guid> interesesIds, CancellationToken cancellationToken);

    /// <summary>
    /// Obtiene las habilidades de un usuario dado su Id.
    /// </summary>
    /// <param name="usuarioId">Id del usuario.</param>
    /// <param name="cancellationToken">Token para cancelar la operacion.</param>
    /// <returns>Lista de Ids de habilidades del usuario.</returns>
    Task<List<Guid>> ObtenerHabilidadesUsuarioAsync(Guid usuarioId, CancellationToken cancellationToken);

    /// <summary>
    /// Obtiene los intereses de un usuario dado su Id.
    /// </summary>
    /// <param name="usuarioId">Id del usuario.</param>
    /// <param name="cancellationToken">Token para cancelar la operacion.</param>
    /// <returns>Lista de Ids de intereses del usuario.</returns>
    Task<List<Guid>> ObtenerInteresesUsuarioAsync(Guid usuarioId, CancellationToken cancellationToken);

    /// <summary>
    /// Verifica si un email esta en uso, excluyendo un usuario especifico.
    /// </summary>
    /// <param name="email">Email a verificar.</param>
    /// <param name="excluirUsuarioId">Id del usuario a excluir.</param>
    /// <param name="cancellationToken">Token para cancelar la operacion.</param>
    Task<bool> EmailEnUsoAsync(string email, Guid excluirUsuarioId, CancellationToken cancellationToken);

    /// <summary>
    /// Actualiza la contrasena de un usuario.
    /// </summary>
    /// <param name="usuario">Usuario al que se le actualiza la contrasena.</param>
    /// <param name="newHashedPassword">Nueva contrasena en hash.</param>
    /// <param name="cancellationToken">Token para cancelar la operacion.</param>
    Task ActualizarContrasenaAsync(Usuario usuario, string newHashedPassword, CancellationToken cancellationToken);

    /// <summary>
    /// Verifica si un email existe en la base de datos.
    /// </summary>
    /// <param name="email">Email a verificar.</param>
    /// <param name="cancellationToken">Token para cancelar la operacion.</param>
    Task<bool> ExisteEmailAsync(string email, CancellationToken cancellationToken);

    /// <summary>
    /// Verifica si un nombre de usuario existe en la base de datos.
    /// </summary>
    /// <param name="nombreUsuario">Nombre de usuario a verificar.</param>
    /// <param name="cancellationToken">Token para cancelar la operacion.</param>
    Task<bool> ExisteNombreUsuarioAsync(string nombreUsuario, CancellationToken cancellationToken);
    
    /// <summary>
    /// Obtiene un usuario con toda su información relacionada, como es intereses y sus habilidades.
    /// </summary>
    /// <param name="usuarioId">Identificador único del usuario.</param>
    /// <param name="cancellationToken">Token para cancelar la operación de manera asíncrona si es necesario.</param>
    /// <returns>Un usuario con sus detalles completos.</returns>
    Task<Usuario> ObtenerDetallesUsuarioPorIdAsync(Guid usuarioId, CancellationToken cancellationToken);

    /// <summary>
    /// Obtiene un usuario específico junto con sus intereses.
    /// </summary>
    /// <param name="usuarioId">El identificador único del usuario.</param>
    /// <param name="cancellationToken">Token para cancelar la operación asíncrona.</param>
    /// <returns>Un usuario con sus intereses o null si no se encuentra.</returns>
    Task<Usuario?> ObtenerUsuarioConInteresYHabilidades(Guid usuarioId, CancellationToken cancellationToken);

    /// <summary>
    /// Obtiene todos los usuarios incluyendo sus intereses.
    /// </summary>
    /// <param name="cancellationToken">Token para cancelar la operación asíncrona.</param>
    /// <returns>Una colección de usuarios con sus intereses.</returns>
    Task<IEnumerable<Usuario>> ObtenerTodosUsuariosConInteresesYHabilidades(CancellationToken cancellationToken);


    Task<IEnumerable<UsuarioInteres>> ObtenerInteresesPorUsuarioIdAsync(Guid usuarioId,
        CancellationToken cancellationToken);
    
    Task<IEnumerable<UsuarioHabilidad>> ObtenerHabilidadesPorUsuarioIdAsync(Guid usuarioId,
        CancellationToken cancellationToken);
    
    Task<IEnumerable<Usuario>> ObtenerUsuariosObjetivoAsync(Guid usuarioActualId, string rol,
        CancellationToken cancellationToken);
    
    Task<string> ObtenerRolDeUsuarioAsync(Guid usuarioId, CancellationToken cancellationToken);
}