using Trivo.Dominio.Modelos;

namespace Trivo.Aplicacion.Interfaces.Repositorio;

/// <summary>
/// Define las operaciones para gestionar la asociación entre usuarios e intereses.
/// </summary>
public interface IRepositorioUsuarioInteres
{
    /// <summary>
    /// Crea una nueva relación entre usuario e interés.
    /// </summary>
    /// <param name="usuarioInteres">Entidad de relación UsuarioInteres que contiene los IDs necesarios.</param>
    /// <param name="cancellationToken">Token de cancelación.</param>
    Task CrearUsuarioInteresAsync(UsuarioInteres usuarioInteres, CancellationToken cancellationToken);
    
    /// <summary>
    /// Crea múltiples relaciones entre usuarios e intereses en la base de datos.
    /// </summary>
    /// <param name="relaciones">Lista de objetos <see cref="UsuarioInteres"/> que representan las relaciones a crear.</param>
    /// <param name="cancellationToken">Token para cancelar la operación asincrónica si es necesario.</param>
    /// <returns>Una tarea que representa la operación asincrónica.</returns>
    Task CrearMultiplesUsuarioInteresAsync(List<UsuarioInteres> relaciones, CancellationToken cancellationToken);

    /// <summary>
    /// Asocia una lista de intereses a un usuario específico, reemplazando las asociaciones existentes.
    /// </summary>
    /// <param name="usuarioId">Identificador único del usuario.</param>
    /// <param name="interesesIds">Lista de identificadores de intereses a asociar con el usuario.</param>
    /// <param name="cancellationToken">Token para cancelar la operación asincrónica si es necesario.</param>
    /// <returns>
    /// Una tarea que representa la operación asincrónica y retorna la lista actualizada
    /// de asociaciones entre el usuario y sus intereses.
    /// </returns>
    Task<List<UsuarioInteres>> AsociarInteresesAlUsuarioAsync(Guid usuarioId, List<Guid> interesesIds, CancellationToken cancellationToken);
}
