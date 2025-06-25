using Trivo.Aplicacion.Paginacion;
using Trivo.Dominio.Modelos;

namespace Trivo.Aplicacion.Interfaces.Repositorio;

public interface IRepositorioHabilidad
{
    /// <summary>
    /// Crea una colección de habilidades en la base de datos.
    /// </summary>
    /// <param name="habilidades">Lista de habilidades a crear.</param>
    /// <param name="cancellationToken">Token para cancelar la operación asincrónica.</param>
    /// <returns>Una tarea que representa la operación asincrónica.</returns>
    Task CrearHabilidadAsync(IEnumerable<Habilidad> habilidades, CancellationToken cancellationToken);

    /// <summary>
    /// Actualiza la lista de habilidades asociadas a un usuario específico.
    /// </summary>
    /// <param name="usuarioId">Identificador único del usuario cuyas habilidades se actualizarán.</param>
    /// <param name="habilidadIds">Lista de identificadores de habilidades que se asignarán al usuario.</param>
    /// <param name="cancellationToken">Token para cancelar la operación asincrónica.</param>
    /// <returns>Una tarea que representa la operación asincrónica.</returns>
    Task ActualizarHabilidadAsync(Guid usuarioId, List<Guid> habilidadIds, CancellationToken cancellationToken);

    /// <summary>
    /// Obtiene una lista paginada de habilidades.
    /// </summary>
    /// <param name="numeroPagina">Número de la página a obtener (comenzando en 1).</param>
    /// <param name="tamanoPagina">Cantidad de elementos por página.</param>
    /// <param name="cancellationToken">Token para cancelar la operación asincrónica si es necesario.</param>
    /// <returns>Una tarea que retorna un resultado paginado con las habilidades solicitadas.</returns>
    Task<ResultadoPaginado<Habilidad>> ObtenerHabilidadPaginadoAsync(int numeroPagina, int tamanoPagina, CancellationToken cancellationToken);
    
    /// <summary>
    /// Obtiene una habilidad por su identificador único.
    /// </summary>
    /// <param name="habilidadId">Identificador de la habilidad a obtener.</param>
    /// <param name="cancellationToken">Token para cancelar la operación asincrónica.</param>
    /// <returns>Una tarea que retorna la habilidad solicitada.</returns>
    Task<Habilidad> ObtenerHabilidadPorIdAsync(Guid habilidadId, CancellationToken cancellationToken);

    /// <summary>
    /// Obtiene los usuarios que tienen asignadas las habilidades indicadas.
    /// </summary>
    /// <param name="habilidadId">Colección de identificadores de habilidades para filtrar usuarios.</param>
    /// <param name="cancellationToken">Token para cancelar la operación asincrónica.</param>
    /// <returns>Una tarea que retorna la colección de usuarios que poseen alguna de las habilidades indicadas.</returns>
    Task<IEnumerable<Usuario>> ObtenerUsuariosPorHabilidadesAsync(IEnumerable<Guid> habilidadId, CancellationToken cancellationToken);
}