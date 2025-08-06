using FluentValidation;

namespace Trivo.Aplicacion.Modulos.Reportes.Commands.Crear;

public class CrearReporteValidacion : AbstractValidator<CrearReporteCommand>
{
    public CrearReporteValidacion()
    {
        RuleFor(x => x.ReportadoPor)
            .NotNull().WithMessage("El ID del usuario que reporta es obligatorio.")
            .NotEqual(Guid.Empty).WithMessage("El ID del usuario que reporta no puede estar vacío.");

        RuleFor(x => x.MensajeId)
            .NotNull().WithMessage("El ID del mensaje a reportar es obligatorio.")
            .NotEqual(Guid.Empty).WithMessage("El ID del mensaje a reportar no puede estar vacío.");

        RuleFor(x => x.NotaReporte)
            .NotEmpty().WithMessage("Debe proporcionar una nota para el reporte.")
            .MaximumLength(250).WithMessage("La nota del reporte no debe superar los 250 caracteres.");
    }
    
}