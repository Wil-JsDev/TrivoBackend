using Trivo.Dominio.Enum;
using Trivo.Dominio.Modelos;

namespace Trivo.Aplicacion.Interfaces.Repositorio;

/// <summary>
/// Repositorio para manejar la entidad Emparejamiento.
/// Incluye métodos específicos para consultar emparejamientos entre expertos y reclutadores.
/// </summary>
public interface IRepositorioEmparejamiento : IRepositorioGenerico<Emparejamiento>
{
    /// <summary>
    /// Obtiene un emparejamiento entre un experto y un reclutador si existe.
    /// </summary>
    /// <param name="expertoId">ID del experto.</param>
    /// <param name="reclutadorId">ID del reclutador.</param>
    /// <param name="cancellationToken">Token de cancelación.</param>
    /// <returns>El emparejamiento encontrado o null si no existe.</returns>
    Task<Emparejamiento?> ObtenerEmparejamientoAsync(Guid expertoId, Guid reclutadorId,
        CancellationToken cancellationToken);

    /// <summary>
    /// Verifica si existe un emparejamiento entre un experto y un reclutador.
    /// </summary>
    /// <param name="expertoId">ID del experto.</param>
    /// <param name="reclutadorId">ID del reclutador.</param>
    /// <param name="cancellationToken">Token de cancelación.</param>
    /// <returns>True si existe el emparejamiento; false en caso contrario.</returns>
    Task<bool> ExisteEmparejamientoAsync(Guid expertoId, Guid reclutadorId, CancellationToken cancellationToken);

    /// <summary>
    /// Obtiene los emparejamientos pendientes en los que el usuario actúa como experto.
    /// </summary>
    /// <param name="usuarioId">Identificador único del usuario.</param>
    /// <param name="cancellationToken">Token para cancelar la operación asincrónica si es necesario.</param>
    /// <returns>Una lista de emparejamientos pendientes asociados al usuario como experto.</returns>
    Task<IEnumerable<Emparejamiento>> ObtenerEmparejamientosComoExpertoAsync(Guid usuarioId, CancellationToken cancellationToken);

    /// <summary>
    /// Obtiene los emparejamientos pendientes en los que el usuario actúa como reclutador.
    /// </summary>
    /// <param name="usuarioId">Identificador único del usuario.</param>
    /// <param name="cancellationToken">Token para cancelar la operación asincrónica si es necesario.</param>
    /// <returns>Una lista de emparejamientos pendientes asociados al usuario como reclutador.</returns>
    Task<IEnumerable<Emparejamiento>> ObtenerEmparejamientosComoReclutadorAsync(Guid usuarioId, CancellationToken cancellationToken);
}