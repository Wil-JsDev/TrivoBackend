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
    /// Filtra y obtiene los usuarios que tienen al menos uno de los intereses o habilidades especificados.
    /// </summary>
    /// <param name="interesesIds">Lista opcional de IDs de intereses para filtrar.</param>
    /// <param name="habilidadesIds">Lista opcional de IDs de habilidades para filtrar.</param>
    /// <param name="cancellationToken">Token para cancelar la operación asincrónica.</param>
    /// <returns>Una colección de objetos <see cref="Usuario"/> que coinciden con los intereses o habilidades especificados.</returns>
    Task<IEnumerable<Usuario>> FiltrarPorInteresesYHabilidadesAsync(
        List<Guid>? interesesIds,
        List<Guid>? habilidadesIds,
        CancellationToken cancellationToken);
    
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
    
   /// <summary>
    /// Obtiene la lista de intereses asociados a un usuario específico.
    /// </summary>
    /// <param name="usuarioId">El ID del usuario.</param>
    /// <param name="cancellationToken">Token para cancelar la operación asincrónica.</param>
    /// <returns>Una colección de objetos <see cref="UsuarioInteres"/>.</returns>
    Task<IEnumerable<UsuarioInteres>> ObtenerInteresesPorUsuarioIdAsync(Guid usuarioId,
        CancellationToken cancellationToken);

    /// <summary>
    /// Obtiene la lista de habilidades asociadas a un usuario específico.
    /// </summary>
    /// <param name="usuarioId">El ID del usuario.</param>
    /// <param name="cancellationToken">Token para cancelar la operación asincrónica.</param>
    /// <returns>Una colección de objetos <see cref="UsuarioHabilidad"/>.</returns>
    Task<IEnumerable<UsuarioHabilidad>> ObtenerHabilidadesPorUsuarioIdAsync(Guid usuarioId,
        CancellationToken cancellationToken);

    /// <summary>
    /// Obtiene la lista de usuarios potenciales para emparejamiento, basada en el rol y el ID del usuario actual.
    /// </summary>
    /// <param name="usuarioActualId">El ID del usuario que realiza la búsqueda.</param>
    /// <param name="rol">El rol del usuario para filtrar los objetivos (por ejemplo: "Experto", "Aprendiz").</param>
    /// <param name="cancellationToken">Token para cancelar la operación asincrónica.</param>
    /// <returns>Una colección de objetos <see cref="Usuario"/> como objetivos posibles.</returns>
    Task<IEnumerable<Usuario>> ObtenerUsuariosObjetivoAsync(Guid usuarioActualId, string rol,
        CancellationToken cancellationToken);

    /// <summary>
    /// Obtiene el rol asignado a un usuario específico.
    /// </summary>
    /// <param name="usuarioId">El ID del usuario.</param>
    /// <param name="cancellationToken">Token para cancelar la operación asincrónica.</param>
    /// <returns>Una cadena que representa el rol del usuario.</returns>
    Task<string> ObtenerRolDeUsuarioAsync(Guid usuarioId, CancellationToken cancellationToken);

    
    Task<Usuario?> ObtenerPorIdConRelacionesAsync(Guid id, CancellationToken cancellationToken);
}