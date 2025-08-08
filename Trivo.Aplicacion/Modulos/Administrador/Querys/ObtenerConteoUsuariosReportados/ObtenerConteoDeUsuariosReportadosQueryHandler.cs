using Microsoft.Extensions.Logging;
using Trivo.Aplicacion.Abstracciones.Mensajes;
using Trivo.Aplicacion.DTOs.Reportes;
using Trivo.Aplicacion.Interfaces.Repositorio.Cuenta;
using Trivo.Aplicacion.Utilidades;

namespace Trivo.Aplicacion.Modulos.Administrador.Querys.ObtenerConteoUsuariosReportados;

internal sealed class ObtenerConteoDeUsuariosReportadosQueryHandler(
    ILogger<ObtenerConteoDeUsuariosReportadosQueryHandler> logger,
    IRepositorioAdministrador repositorioAdministrador
    ) : IQueryHandler<ObtenerConteoDeUsuariosReportadosQuery, ConteoUsuariosReportados>
{
    public async Task<ResultadoT<ConteoUsuariosReportados>> Handle(ObtenerConteoDeUsuariosReportadosQuery request, CancellationToken cancellationToken)
    {
        var usuariosReportados = await repositorioAdministrador.ObtenerConteoUsuariosReportados(cancellationToken);
        if (usuariosReportados == 0)
        {
            logger.LogWarning("No se encontraron usuarios reportados.");
            
            return ResultadoT<ConteoUsuariosReportados>.Fallo(Error.NoEncontrado("404","No se encontraron usuarios reportados"));
        }
        logger.LogInformation("Se encontraron {ConteoUsuariosReportados} usuarios reportados.", usuariosReportados);
        
        return ResultadoT<ConteoUsuariosReportados>.Exito(new ConteoUsuariosReportados(usuariosReportados));
    }
}