using Trivo.Dominio.Modelos;

namespace Trivo.Aplicacion.Interfaces.Repositorio.Cuenta;

public interface IRepositorioCodigo
{
/// <summary>
/// Crea un nuevo codigo.
/// </summary>
/// <param name="codigo">El objeto codigo a crear.</param>
/// <param name="cancellationToken">Token para cancelar la operacion.</param>
Task CrearCodigoAsync(Codigo codigo, CancellationToken cancellationToken);

/// <summary>
/// Obtiene un codigo por su identificador.
/// </summary>
/// <param name="id">El identificador del codigo.</param>
/// <param name="cancellationToken">Token para cancelar la operacion.</param>
/// <returns>El codigo encontrado o null si no existe.</returns>
Task<Codigo> ObtenerCodigoPorIdAsync(Guid id, CancellationToken cancellationToken);

/// <summary>
/// Busca un codigo por su valor.
/// </summary>
/// <param name="codigo">El valor del codigo a buscar.</param>
/// <param name="cancellationToken">Token para cancelar la operacion.</param>
/// <returns>El codigo encontrado o null si no existe.</returns>
Task<Codigo> BuscarCodigoAsync(string codigo, CancellationToken cancellationToken);

/// <summary>
/// Elimina un codigo.
/// </summary>
/// <param name="codigo">El objeto codigo a eliminar.</param>
/// <param name="cancellationToken">Token para cancelar la operacion.</param>
Task EliminarCodigoAsync(Codigo codigo, CancellationToken cancellationToken);

/// <summary>
/// Verifica si existe un codigo especifico.
/// </summary>
/// <param name="codigo">El valor del codigo a verificar.</param>
/// <param name="cancellationToken">Token para cancelar la operacion.</param>
/// <returns>True si existe, false en caso contrario.</returns>
Task<bool> ExisteElCodigoAsync(string codigo, CancellationToken cancellationToken);

/// <summary>
/// Verifica si un codigo es valido.
/// </summary>
/// <param name="codigo">El valor del codigo a validar.</param>
/// <param name="cancellationToken">Token para cancelar la operacion.</param>
/// <returns>True si el codigo es valido, false en caso contrario.</returns>
Task<bool> ElCodigoEsValidoAsync(string codigo, CancellationToken cancellationToken);

/// <summary>
/// Marca un codigo como usado.
/// </summary>
/// <param name="codigo">El valor del codigo a marcar.</param>
/// <param name="cancellationToken">Token para cancelar la operacion.</param>
Task MarcarCodigoComoUsado(string codigo, CancellationToken cancellationToken);

/// <summary>
/// Verifica si un codigo no ha sido usado aun.
/// </summary>
/// <param name="codigo">El valor del codigo a verificar.</param>
/// <param name="cancellationToken">Token para cancelar la operacion.</param>
/// <returns>True si el codigo no ha sido usado, false en caso contrario.</returns>
Task<bool> CodigoNoUsadoAsync(string codigo, CancellationToken cancellationToken);

}