using FluentValidation;

namespace Trivo.Aplicacion.Modulos.Emparejamiento.Commands.Crear;

public class CrearEmparejamientoValidacion : AbstractValidator<CrearEmparejamientoCommand>
{
    public CrearEmparejamientoValidacion()
    {
        RuleFor(x => x.ExpertoId)
            .NotNull().WithMessage("El ID del experto es obligatorio.")
            .Must(id => id != Guid.Empty).WithMessage("El ID del experto debe ser un GUID válido.");

        RuleFor(x => x.ReclutadorId)
            .NotNull().WithMessage("El ID del reclutador es obligatorio.")
            .Must(id => id != Guid.Empty).WithMessage("El ID del reclutador debe ser un GUID válido.");
        
        RuleFor(x => x.CreadoPor)
            .NotEmpty().WithMessage("Debe especificarse quién creó el emparejamiento.");
    }
}