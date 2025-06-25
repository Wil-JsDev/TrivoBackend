using Trivo.Aplicacion.Paginacion;
using Trivo.Dominio.Modelos;

namespace Trivo.Aplicacion.Interfaces.Repositorio;

/// <summary>
/// Define las operaciones para gestionar intereses dentro del sistema.
/// </summary>
public interface IRepositorioInteres
{
    /// <summary>
    /// Crea una lista de intereses en la base de datos.
    /// </summary>
    /// <param name="interes">Colección de intereses a crear.</param>
    /// <param name="cancellationToken">Token para cancelar la operación de manera asíncrona si es necesario.</param>
    /// <returns>Una tarea que representa la operación asincrónica.</returns>
    Task CrearInteresAsync(IEnumerable<Interes> interes, CancellationToken cancellationToken);

    /// <summary>
    /// Actualiza los intereses asociados a un usuario específico.
    /// </summary>
    /// <param name="usuarioId">ID del usuario a actualizar.</param>
    /// <param name="interesIds">IDs de los nuevos intereses que tendrá el usuario.</param>
    /// <param name="cancellationToken">Token de cancelación.</param>
    /// <returns>Una tarea asincrónica.</returns>
    Task ActualizarInteresesDeUsuarioAsync(Guid usuarioId, List<Guid> interesIds, CancellationToken cancellationToken);

    /// <summary>
    /// Obtiene una lista paginada de intereses existentes en la base de datos.
    /// </summary>
    /// <param name="numeroPagina">Número de la página a consultar (comenzando en 1).</param>
    /// <param name="tamanoPagina">Cantidad de elementos por página.</param>
    /// <param name="cancellationToken">Token de cancelación para la operación asincrónica.</param>
    /// <returns>Una tarea que retorna un resultado paginado con los intereses.</returns>
    Task<ResultadoPaginado<Interes>> ObtenerInteresesPaginadoAsync(int numeroPagina, int tamanoPagina, CancellationToken cancellationToken);

    /// <summary>
    /// Obtiene un interés específico por su identificador único.
    /// </summary>
    /// <param name="interesId">Identificador del interés a buscar.</param>
    /// <param name="cancellationToken">Token de cancelación para la operación asincrónica.</param>
    /// <returns>Una tarea que retorna el interés encontrado o null si no existe.</returns>
    Task<Interes> ObtenerPorIdAsync(Guid interesId, CancellationToken cancellationToken);
    
    /// <summary>
    /// Obtiene los usuarios que están asociados con alguno de los intereses indicados.
    /// </summary>
    /// <param name="interesesId">Colección de identificadores de intereses para filtrar usuarios.</param>
    /// <param name="cancellationToken">Token para cancelar la operación asincrónica si es necesario.</param>
    /// <returns>Una tarea que retorna una colección de usuarios asociados a los intereses proporcionados.</returns>
    Task<IEnumerable<Usuario>> ObtenerUsuariosPorInteresesAsync(IEnumerable<Guid> interesesId, CancellationToken cancellationToken);
}
