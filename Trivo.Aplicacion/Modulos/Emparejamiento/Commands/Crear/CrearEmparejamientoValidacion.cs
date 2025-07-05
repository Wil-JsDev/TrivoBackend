using FluentValidation;

namespace Trivo.Aplicacion.Modulos.Emparejamiento.Commands.Crear;

public class CrearEmparejamientoValidacion : AbstractValidator<CrearEmparejamientoCommand>
{
    public CrearEmparejamientoValidacion()
    {
        RuleFor(x => x.ExpertoId)
            .NotEmpty()
            .WithMessage("El ID del experto es obligatorio");
        
        RuleFor(x => x.ReclutadorId)
            .NotEmpty()
            .WithMessage("El ID del reclutador es obligatorio");
    }
}