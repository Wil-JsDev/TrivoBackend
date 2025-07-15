using Trivo.Aplicacion.Utilidades;

namespace Trivo.Aplicacion.Interfaces.Servicios;

public interface IValidarCorreo
{
    Task<ResultadoT<bool>> ValidarCorreoAsync(string email, CancellationToken cancellationToken);
}