using Microsoft.Extensions.Logging;
using Serilog;
using Trivo.Aplicacion.Abstracciones.Mensajes;
using Trivo.Aplicacion.DTOs.Emparejamiento;
using Trivo.Aplicacion.Interfaces.Repositorio.Cuenta;
using Trivo.Aplicacion.Utilidades;

namespace Trivo.Aplicacion.Modulos.Administrador.Querys.ObtenerConteoDeEmparejamientos;
internal sealed class ObtenerConteoEmparejamientosCompletadosQueryHandler(
    ILogger<ObtenerConteoEmparejamientosCompletadosQueryHandler> logger,
    IRepositorioAdministrador repositorioAdministrador
    ) : IQueryHandler<ObtenerConteoEmparejamientosCompletadosQuery, ConteoEmparejamientoCompletadosDto>
{
    public async Task<ResultadoT<ConteoEmparejamientoCompletadosDto>> Handle(ObtenerConteoEmparejamientosCompletadosQuery request, CancellationToken cancellationToken)
    {
        var conteoEmparejamiento =
            await repositorioAdministrador.ContarEmparejamientosCompletadosAsync(cancellationToken);

        if (conteoEmparejamiento == 0)
        {
            logger.LogWarning("No se encontraron emparejamientos completados.");
            
            return ResultadoT<ConteoEmparejamientoCompletadosDto>.Fallo(Error.NoEncontrado("404","No se encontraron match completados"));
        }
        
        logger.LogInformation("Se encontraron {ConteoEmparejamiento} emparejamientos completados.", conteoEmparejamiento);
        
        return ResultadoT<ConteoEmparejamientoCompletadosDto>.Exito(new ConteoEmparejamientoCompletadosDto(conteoEmparejamiento));
    }
}