using Microsoft.Extensions.Logging;
using Trivo.Aplicacion.Abstracciones.Mensajes;
using Trivo.Aplicacion.DTOs.Reportes;
using Trivo.Aplicacion.Interfaces.Repositorio;
using Trivo.Aplicacion.Interfaces.Repositorio.Cuenta;
using Trivo.Aplicacion.Utilidades;
using Trivo.Dominio.Enum;
using Trivo.Dominio.Modelos;

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
            logger.LogInformation("No se encontró el usuario con ID: {UsuarioId}", request.ReportadoPor);
    
            return ResultadoT<ReporteDto>.Fallo(Error.NoEncontrado("400", "El usuario que reporta no fue encontrado."));
        }

        var mensaje = await repositorioMensaje.ObtenerByIdAsync(request.MensajeId ?? Guid.Empty, cancellationToken);
        if (mensaje is null)
        {
            logger.LogInformation("No se encontró el mensaje con ID: {MensajeId}", request.MensajeId);
    
            return ResultadoT<ReporteDto>.Fallo(Error.NoEncontrado("400", "El mensaje a reportar no fue encontrado."));
        }

        if (string.IsNullOrEmpty(request.NotaReporte))
        {
            logger.LogInformation("La nota del reporte está vacía o es nula.");
    
            return ResultadoT<ReporteDto>.Fallo(Error.Fallo("400", "Debe proporcionar una nota para el reporte."));
        }

        // Cargar las relaciones de emisor y receptor
        var mensajeConUsuarios = await repositorioMensaje.ObtenerUsuarioQuePerteneceElMensajeAsync(request.MensajeId ?? Guid.Empty, cancellationToken);
        logger.LogInformation("Relaciones de usuarios del mensaje cargadas correctamente para el mensaje ID: {MensajeId}", request.MensajeId);

        Reporte reporteEntidad = new()
        {
            ReporteId = Guid.NewGuid(),
            ReportadoPor = request.ReportadoPor,
            MensajeId = request.MensajeId,
            EstadoReporte = EstadoReporte.Pendiente.ToString(),
            Nota = request.NotaReporte
        };

        await repositorioReporte.CrearAsync(reporteEntidad, cancellationToken);
        
        logger.LogInformation("Reporte creado exitosamente con ID: {ReporteId}", reporteEntidad.ReporteId);
        
        var usuarioReportado = mensajeConUsuarios!.EmisorId == usuario.Id
            ? mensajeConUsuarios.Receptor
            : mensajeConUsuarios.Emisor;
        
        var mensajeDto = new MensajeDtoParaReporte
        (
            MensajeId: mensajeConUsuarios.MensajeId,
            EmisorId: mensajeConUsuarios.EmisorId,
            Contenido: mensajeConUsuarios.Contenido,
            Tipo: mensajeConUsuarios.Tipo,
            FechaEnvio: mensajeConUsuarios.FechaEnvio,
            Emisor: mensajeConUsuarios.Emisor is null ? null : new UsuarioDtoParaReporte(
                UsuarioId: mensajeConUsuarios.Emisor.Id ?? Guid.Empty,
                Nombre: mensajeConUsuarios.Emisor.Nombre,
                Apellido: mensajeConUsuarios.Emisor.Apellido
            )
        );

        var usuarioReportadoPorDto = new UsuarioDtoParaReporte(
            UsuarioId: usuario.Id ?? Guid.Empty,
            Nombre: usuario.Nombre,
            Apellido: usuario.Apellido
        );

        var usuarioReportadoDto = usuarioReportado is null ? null : new UsuarioDtoParaReporte(
            UsuarioId: usuarioReportado.Id ?? Guid.Empty,
            Nombre: usuarioReportado.Nombre!,
            Apellido: usuarioReportado.Apellido!
        );

        var reporteDto = new ReporteDto
        (
            ReporteId: reporteEntidad.ReporteId ?? Guid.Empty,
            ReportadoPor: reporteEntidad.ReportadoPor,
            MensajeId: reporteEntidad.MensajeId,
            Nota: reporteEntidad.Nota,
            EstadoReporte: reporteEntidad.EstadoReporte,
            Mensaje: mensajeDto,
            UsuarioReportadoPor: usuarioReportadoPorDto,
            UsuarioReportado: usuarioReportadoDto
        );

        return ResultadoT<ReporteDto>.Exito(reporteDto);
    }
}