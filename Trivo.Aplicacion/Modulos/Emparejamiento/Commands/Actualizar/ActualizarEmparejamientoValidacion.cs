using FluentValidation;
using Trivo.Dominio.Enum;

namespace Trivo.Aplicacion.Modulos.Emparejamiento.Commands.Actualizar;

public class ActualizarEmparejamientoValidacion : AbstractValidator<ActualizarEmparejamientoCommand>
{
    public ActualizarEmparejamientoValidacion()
    {
        RuleFor(x => x.EmparejamientoId)
            .NotEmpty().WithMessage("El ID de emparejamiento es requerido")
            .WithErrorCode("400");

        RuleFor(x => x.FaltaPorEmparejamiento)
            .NotNull().WithMessage("Debe especificar quién realiza la actualización (Experto o Reclutador)")
            .IsInEnum().WithMessage("Rol no válido para la actualización")
            .WithErrorCode("400");

        RuleFor(x => x.Estado)
            .NotNull().WithMessage("El estado de actualización es requerido")
            .IsInEnum().WithMessage("Estado de emparejamiento no válido")
            .WithErrorCode("400");

    }
}