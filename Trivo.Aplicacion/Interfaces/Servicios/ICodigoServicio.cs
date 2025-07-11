using Trivo.Aplicacion.DTOs.Email;
using Trivo.Aplicacion.Utilidades;
using Trivo.Dominio.Enum;

namespace Trivo.Aplicacion.Interfaces.Servicios;

public interface ICodigoServicio
{
    Task<ResultadoT<string>> GenerarCodigoAsync(Guid usuarioId,TipoCodigo tipoCodigo,CancellationToken cancellationToken);
    
    Task<ResultadoT<CodigoDto>> ObtenerCodigoAsync(Guid codigoId, CancellationToken cancellationToken);
    
    Task<Resultado> BorrarCodigoAsync(Guid codigoId, CancellationToken cancellationToken);
  
    Task<Resultado> ConfirmarCuentaAsync(Guid usuarioId, string codigo, CancellationToken cancellationToken);
    
    Task<Resultado> CodigoDisponibleAsync(string codigo, CancellationToken cancellationToken);
}
