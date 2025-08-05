using Microsoft.Extensions.Logging;
using Trivo.Aplicacion.Abstracciones.Mensajes;
using Trivo.Aplicacion.DTOs.Reportes;
using Trivo.Aplicacion.Interfaces.Repositorio;
using Trivo.Aplicacion.Interfaces.Repositorio.Cuenta;
using Trivo.Aplicacion.Utilidades;

namespace Trivo.Aplicacion.Modulos.Reportes.Commands.Crear;

internal sealed class CrearReporteCommandHandler(
    ILogger<CrearReporteCommandHandler> logger,
    IRepositorioReporte repositorioReporte,
    IRepositorioMensaje repositorioMensaje,
    IRepositorioUsuario repositorioUsuario
    ) : ICommandHandler<CrearReporteCommand, ReporteDto>
{
    public async Task<ResultadoT<ReporteDto>> Handle(CrearReporteCommand request, CancellationToken cancellationToken)
    {
        var usuario = await repositorioUsuario.ObtenerByIdAsync(request.ReportadoPor ?? Guid.Empty, cancellationToken);
        if (usuario is null)
        {
            logger.LogInformation("");
            
            return ResultadoT<ReporteDto>.Fallo(Error.NoEncontrado("400",""));
        }
        
        var mensaje = await repositorioMensaje.ObtenerByIdAsync(request.MensajeId ?? Guid.Empty, cancellationToken);
        if (mensaje is null)
        {
            logger.LogInformation("");
            
            return ResultadoT<ReporteDto>.Fallo(Error.NoEncontrado("400",""));
        }

        if (string.IsNullOrEmpty(request.NotaReporte))
        {
            logger.LogInformation("");
            
            return ResultadoT<ReporteDto>.Fallo(Error.Fallo("400",""));
        }
        
        //Cargar las relaciones de emisor y receptor
        var mensajeConUsuarios = await repositorioMensaje.ObtenerUsuarioQuePerteneceElMensajeAsync(request.MensajeId ?? Guid.Empty, cancellationToken);
        
        
        
    }
}