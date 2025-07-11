using Trivo.Aplicacion.Interfaces.Base;
using Trivo.Aplicacion.Paginacion;
using Trivo.Dominio.Modelos;

namespace Trivo.Aplicacion.Interfaces.Repositorio;

/// <summary>
/// Define las operaciones para gestionar intereses dentro del sistema.
/// </summary>
public interface IRepositorioInteres : IValidacion<Interes>
{
    /// <summary>
    /// Crea una lista de intereses en la base de datos.
    /// </summary>
    /// <param name="interes">Colección de intereses a crear.</param>
    /// <param name="cancellationToken">Token para cancelar la operación de manera asíncrona si es necesario.</param>
    /// <returns>Una tarea que representa la operación asincrónica.</returns>
    Task CrearInteresAsync(Interes interes, CancellationToken cancellationToken);

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
    /// <returns>
    /// Una tarea que representa la operación asincrónica. El resultado contiene un objeto 
    /// <see cref="ResultadoPaginado{Interes}"/> con los intereses correspondientes a la página solicitada.
    /// </returns>
    Task<ResultadoPaginado<Interes>> ObtenerInteresesPaginadosAsync(int numeroPagina, int tamanoPagina, CancellationToken cancellationToken);

    /// <summary>
    /// Obtiene una lista de usuarios que están asociados a intereses pertenecientes a una categoría específica.
    /// </summary>
    /// <param name="categoriaId">Identificador único de la categoría de interés.</param>
    /// <param name="cancellationToken">Token de cancelación para la operación asincrónica.</param>
    /// <returns>
    /// Una tarea que representa la operación asincrónica. El resultado contiene una colección de usuarios
    /// relacionados con al menos un interés dentro de la categoría especificada.
    /// </returns>
    Task<IEnumerable<Usuario>> ObtenerUsuariosPorCategoriaDeInteresAsync(Guid categoriaId, CancellationToken cancellationToken);

    /// <summary>
    /// Obtiene una lista paginada de intereses filtrados por una categoría específica.
    /// </summary>
    /// <param name="categoriaId">Identificador único de la categoría a la que pertenecen los intereses.</param>
    /// <param name="numeroPagina">Número de la página a consultar (comenzando en 1).</param>
    /// <param name="tamanoPagina">Cantidad de elementos por página.</param>
    /// <param name="cancellationToken">Token de cancelación para la operación asincrónica.</param>
    /// <returns>
    /// Una tarea que representa la operación asincrónica. El resultado contiene un objeto
    /// <see cref="ResultadoPaginado{Interes}"/> con los intereses asociados a la categoría indicada.
    /// </returns>
    Task<ResultadoPaginado<Interes>> ObtenerInteresesPorCategoriaPaginadosAsync(IEnumerable<Guid> categoriaId, int numeroPagina, int tamanoPagina, CancellationToken cancellationToken);
    
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

    /// <summary>
    /// Obtiene una colección de intereses que pertenecen a una o varias categorías específicas.
    /// </summary>
    /// <param name="numeroPagina"></param>
    /// <param name="tamanoPagina"></param>
    /// <param name="cancellationToken">Token para cancelar la operación asincrónica.</param>
    /// <param name="categoriaIds"></param>
    /// <returns>Una tarea que representa la operación asincrónica y contiene una colección de intereses.</returns>
    Task<ResultadoPaginado<Interes>> ObtenerInteresPorICategoriaIdAsync(
        IEnumerable<Guid> categoriaIds,
        int numeroPagina,
        int tamanoPagina,
        CancellationToken cancellationToken);

    /// <summary>
    /// Verifica si ya existe un interés con el nombre especificado, sin importar la categoría.
    /// </summary>
    /// <param name="nombre">Nombre del interés a validar.</param>
    /// <param name="cancellationToken">Token para cancelar la operación.</param>
    /// <returns>
    /// <c>true</c> si ya existe un interés con ese nombre; de lo contrario, <c>false</c>.
    /// </returns>
    Task<bool> NombreExisteAsync(string nombre, CancellationToken cancellationToken);

    /// <summary>
    /// Verifica si ya existe un interés con el nombre especificado dentro de una categoría específica.
    /// </summary>
    /// <param name="nombre">Nombre del interés a validar.</param>
    /// <param name="categoriaId">ID de la categoría donde se validará la existencia del interés.</param>
    /// <param name="cancellationToken">Token para cancelar la operación.</param>
    /// <returns>
    /// <c>true</c> si ya existe un interés con ese nombre en la categoría especificada; de lo contrario, <c>false</c>.
    /// </returns>
    Task<bool> NombreCategoriaExisteAsync(string nombre, Guid categoriaId, CancellationToken cancellationToken);
    
    /// <summary>
    /// Busca intereses cuyo nombre contenga el texto especificado, sin importar mayúsculas o minúsculas.
    /// </summary>
    /// <param name="interes">Texto a buscar dentro del nombre de los intereses.</param>
    /// <param name="cancellationToken">Token para cancelar la operación de manera anticipada.</param>
    /// <returns>Una colección de intereses que coinciden parcialmente con el texto proporcionado.</returns>
    Task<IEnumerable<Interes>> BuscarInteresesPorNombreAsync(string interes, CancellationToken cancellationToken);

}
