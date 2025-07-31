using Trivo.Aplicacion.DTOs.Email;
using Trivo.Aplicacion.Utilidades;
using Trivo.Dominio.Enum;

namespace Trivo.Aplicacion.Interfaces.Servicios;

/// <summary>
/// Servicio responsable de la generación, validación y gestión de códigos para los usuarios.
/// </summary>
public interface ICodigoServicio
{
    /// <summary>
    /// Genera un nuevo código para un usuario con un tipo específico (ej. confirmación de cuenta, recuperación de contraseña).
    /// </summary>
    /// <param name="usuarioId">ID del usuario al que se le generará el código.</param>
    /// <param name="tipoCodigo">Tipo de código a generar.</param>
    /// <param name="cancellationToken">Token para cancelar la operación.</param>
    /// <returns>Resultado que contiene el código generado como string.</returns>
    Task<ResultadoT<string>> GenerarCodigoAsync(Guid usuarioId, TipoCodigo tipoCodigo, CancellationToken cancellationToken);

    /// <summary>
    /// Obtiene los detalles de un código a partir de su ID.
    /// </summary>
    /// <param name="codigoId">ID del código a buscar.</param>
    /// <param name="cancellationToken">Token para cancelar la operación.</param>
    /// <returns>Resultado que contiene el DTO con los datos del código.</returns>
    Task<ResultadoT<CodigoDto>> ObtenerCodigoAsync(Guid codigoId, CancellationToken cancellationToken);

    /// <summary>
    /// Elimina un código existente en base a su ID.
    /// </summary>
    /// <param name="codigoId">ID del código a eliminar.</param>
    /// <param name="cancellationToken">Token para cancelar la operación.</param>
    /// <returns>Resultado de la operación.</returns>
    Task<Resultado> BorrarCodigoAsync(Guid codigoId, CancellationToken cancellationToken);

    /// <summary>
    /// Confirma la cuenta de un usuario usando un código de validación.
    /// </summary>
    /// <param name="usuarioId">ID del usuario que desea confirmar su cuenta.</param>
    /// <param name="codigo">Código recibido por el usuario.</param>
    /// <param name="cancellationToken">Token para cancelar la operación.</param>
    /// <returns>Resultado de la operación.</returns>
    Task<Resultado> ConfirmarCuentaAsync(Guid usuarioId, string codigo, CancellationToken cancellationToken);

    /// <summary>
    /// Verifica si un código está disponible (válido y no expirado).
    /// </summary>
    /// <param name="codigo">Código a verificar.</param>
    /// <param name="cancellationToken">Token para cancelar la operación.</param>
    /// <returns>Resultado de la operación.</returns>
    Task<Resultado> CodigoDisponibleAsync(string codigo, CancellationToken cancellationToken);

    /// <summary>
    /// Valida si un código es correcto y está activo.
    /// </summary>
    /// <param name="codigo">Código a validar.</param>
    /// <param name="cancellationToken">Token para cancelar la operación.</param>
    /// <returns>Resultado que indica si el código es válido.</returns>
    Task<ResultadoT<string>> ValidarCodigoAsync(string codigo, CancellationToken cancellationToken);
}
