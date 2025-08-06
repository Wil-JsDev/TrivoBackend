using Trivo.Aplicacion.Abstracciones.Mensajes;
using Trivo.Aplicacion.DTOs.Reportes;

namespace Trivo.Aplicacion.Modulos.Reportes.Commands.Crear;

public sealed record CrearReporteCommand
(
    Guid? ReportadoPor,
    Guid? MensajeId,
    string? NotaReporte
) : ICommand<ReporteDto>;