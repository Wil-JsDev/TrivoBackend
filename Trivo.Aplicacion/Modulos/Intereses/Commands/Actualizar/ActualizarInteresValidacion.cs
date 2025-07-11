using FluentValidation;

namespace Trivo.Aplicacion.Modulos.Intereses.Commands.Actualizar;

public class ActualizarInteresValidacion : AbstractValidator<ActualizarInteresCommand>
{
    public ActualizarInteresValidacion()
    {
        RuleFor(x => x.UsuarioId)
            .NotEmpty().WithMessage("El ID del usuario es obligatorio.");

        RuleFor(x => x.InteresIds)
            .NotNull().WithMessage("Debe proporcionar la lista de intereses.")
            .Must(ids => ids.Any()).WithMessage("Debe proporcionar al menos un interés.")
            .Must(ids => ids.Distinct().Count() == ids.Count)
            .WithMessage("No se permiten intereses duplicados.");
    }
}