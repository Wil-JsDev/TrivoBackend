using FluentValidation;

namespace Trivo.Aplicacion.Modulos.Habilidades.Commands.Actualizar;

public class ActualizarHabilidadValidacion : AbstractValidator<ActualizarHabilidadCommand>
{
    public ActualizarHabilidadValidacion()
    {
        RuleFor(x => x.UsuarioId)
            .NotEmpty().WithMessage("El ID del usuario es obligatorio.");

        RuleFor(x => x.HabilidadIds)
            .NotNull().WithMessage("Debe proporcionar la lista de habilidades.")
            .Must(ids => ids.Any()).WithMessage("Debe proporcionar al menos una habilidad.")
            .Must(ids => ids.Distinct().Count() == ids.Count)
            .WithMessage("No se permiten intereses duplicados.");
    }
}