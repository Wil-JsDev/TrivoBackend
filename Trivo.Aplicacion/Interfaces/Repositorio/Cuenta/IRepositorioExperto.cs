using Trivo.Dominio.Modelos;

namespace Trivo.Aplicacion.Interfaces.Repositorio.Cuenta;

/// <summary>
/// Define las operaciones específicas para la entidad <see cref="Experto"/>, 
/// incluyendo filtrado por habilidades e intereses.
/// </summary>
public interface IRepositorioExperto : IRepositorioGenerico<Experto>
{
    /// <summary>
    /// Filtra expertos que tengan al menos una de las habilidades o intereses especificados.
    /// </summary>
    /// <param name="habilidadesIds">Lista de IDs de habilidades para filtrar.</param>
    /// <param name="interesesIds">Lista de IDs de intereses para filtrar.</param>
    /// <param name="cancellationToken">Token de cancelación para la operación asincrónica.</param>
    /// <returns>
    /// Una colección de expertos que coinciden con los criterios de habilidades e intereses proporcionados.
    /// </returns>
    Task<IEnumerable<Experto>> FiltrarExpertoAsync(
        List<Guid> habilidadesIds, 
        List<Guid> interesesIds, 
        CancellationToken cancellationToken);
    
    /// <summary>
    /// Verifica si el usuario está registrado como experto.
    /// </summary>
    Task<bool> EsUsuarioExpertoAsync(Guid usuarioId, CancellationToken cancellationToken);

    Task<Experto?> ObtenerDetallesExpertoAsync(Guid usuarioId, CancellationToken cancellationToken);
    
    Task<Experto?> ObtenerIdAsync(Guid expertoId, CancellationToken cancellationToken);
}