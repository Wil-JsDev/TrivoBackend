using Trivo.Dominio.Enum;
using Trivo.Dominio.Modelos;

namespace Trivo.Aplicacion.Interfaces.Repositorio;

public interface IRepositorioUsuarioHabilidad
{
    /// <summary>
    /// Crea la asociación entre una habilidad y un usuario específico.
    /// </summary>
    /// <param name="usuarioHabilidad">Entidad para agregar</param>
    /// <param name="cancellationToken">Token para cancelar la operación asincrónica si es necesario.</param>
    /// <returns>Una tarea que representa la operación asincrónica.</returns>
    Task CrearHabilidadesUsuarioAsync(UsuarioHabilidad usuarioHabilidad, CancellationToken cancellationToken);

    /// <summary>
    /// Asocia una lista de habilidades a un usuario, reemplazando las asociaciones existentes.
    /// </summary>
    /// <param name="usuarioId">Identificador del usuario.</param>
    /// <param name="habilidadIds">Lista de identificadores de habilidades a asociar.</param>
    /// <param name="cancellationToken">Token para cancelar la operación asincrónica.</param>
    /// <returns>Una tarea que representa la operación asincrónica.</returns>
    Task<List<UsuarioHabilidad>> AsociarHabilidadesAlUsuarioAsync(Guid usuarioId, List<Guid> habilidadIds,
        CancellationToken cancellationToken);
}