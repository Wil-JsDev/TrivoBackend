using Trivo.Dominio.Modelos;

namespace Trivo.Aplicacion.Interfaces.Repositorio.Cuenta;

public interface IRepositorioReclutador: IRepositorioGenerico<Reclutador>
{
    /// <summary>
    /// Filtra una lista de reclutadores segun habilidades e intereses especificos.
    /// </summary>
    /// <param name="habilidadesIds">Lista de IDs de habilidades a filtrar.</param>
    /// <param name="interesesIds">Lista de IDs de intereses a filtrar.</param>
    /// <param name="cancellationToken">Token para cancelar la operacion asincronica si es necesario.</param>
    /// <returns>Una tarea que representa la operacion asincronica. El resultado contiene una lista de reclutadores que cumplen con los filtros.</returns>
    Task<IEnumerable<Reclutador>> FiltrarReclutadorAsync(
        List<Guid> habilidadesIds, 
        List<Guid> interesesIds, 
        CancellationToken cancellationToken);
    
    /// <summary>
    /// Verifica si el usuario está registrado como reclutador.
    /// </summary>
    Task<bool> EsUsuarioReclutadorAsync(Guid usuarioId, CancellationToken cancellationToken);
    
    Task<Reclutador?> ObtenerDetallesReclutadorAsync(Guid usuarioId, CancellationToken cancellationToken);
}