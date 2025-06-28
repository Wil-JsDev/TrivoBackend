using Trivo.Aplicacion.Interfaces.Base;
using Trivo.Aplicacion.Paginacion;
using Trivo.Dominio.Modelos;

namespace Trivo.Aplicacion.Interfaces.Repositorio;

/// <summary>
/// Define las operaciones para gestionar categorías de interés dentro del sistema.
/// </summary>
public interface IRepositorioCategoriaInteres : IValidacion<CategoriaInteres>
{
    /// <summary>
    /// Crea una colección de categorías de interés en la base de datos.
    /// </summary>
    /// <param name="categorias">Lista de categorías de interés a crear.</param>
    /// <param name="cancellationToken">Token para cancelar la operación asincrónica si es necesario.</param>
    /// <returns>Una tarea que representa la operación asincrónica.</returns>
    Task CrearCategoriaInteresAsync(CategoriaInteres categorias, CancellationToken cancellationToken);

    /// <summary>
    /// Obtiene una categoría de interés específica por su identificador único.
    /// </summary>
    /// <param name="categoriaInteresId">Identificador de la categoría de interés a buscar.</param>
    /// <param name="cancellationToken">Token de cancelación para la operación asincrónica.</param>
    /// <returns>Una tarea que retorna la categoría de interés encontrada.</returns>
    Task<CategoriaInteres> ObtenerPorIdAsync(Guid categoriaInteresId, CancellationToken cancellationToken);

    /// <summary>
    /// Obtiene una lista paginada de categorías de interés existentes en la base de datos.
    /// </summary>
    /// <param name="numeroPagina">Número de la página a consultar (comenzando en 1).</param>
    /// <param name="tamanoPagina">Cantidad de elementos por página.</param>
    /// <param name="cancellationToken">Token de cancelación para la operación asincrónica.</param>
    /// <returns>Una tarea que retorna un resultado paginado con las categorías de interés.</returns>
    Task<ResultadoPaginado<CategoriaInteres>> ObtenerCategoriaInteresPaginadoAsync(int numeroPagina, int tamanoPagina, CancellationToken cancellationToken);
    
    /// <summary>
    /// Actualiza una categoría de interés existente en la base de datos.
    /// </summary>
    /// <param name="categoriaInteres">Entidad de categoría de interés con los nuevos valores.</param>
    /// <param name="cancellationToken">Token de cancelación para la operación asincrónica.</param>
    /// <returns>Una tarea que representa la operación asincrónica.</returns>
    Task ActualizarCategoriaInteresAsync(CategoriaInteres categoriaInteres, CancellationToken cancellationToken);
    
    /// <summary>
    /// Verifica si ya existe un registro con el nombre especificado.
    /// </summary>
    /// <param name="nombre">El nombre a verificar en la base de datos.</param>
    /// <param name="cancellationToken">Token para cancelar la operación si es necesario.</param>
    /// <returns>
    /// <c>true</c> si existe un registro con ese nombre; de lo contrario, <c>false</c>.
    /// </returns>
    Task<bool> NombreExisteAsync(string nombre, CancellationToken cancellationToken);

}
