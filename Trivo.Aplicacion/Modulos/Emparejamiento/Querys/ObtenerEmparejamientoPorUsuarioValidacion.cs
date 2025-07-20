using FluentValidation;
using Trivo.Dominio.Enum;

namespace Trivo.Aplicacion.Modulos.Emparejamiento.Querys;

public class ObtenerEmparejamientoPorUsuarioValidacion : AbstractValidator<ObtenerEmparejamientoPorUsuarioQuery>
{
    public ObtenerEmparejamientoPorUsuarioValidacion()
    {
        RuleFor(x => x.UsuarioId)
            .NotEmpty().WithMessage("El ID del usuario es obligatorio.")
            .Must(id => id != Guid.Empty).WithMessage("El ID del usuario debe ser un GUID válido.");

        RuleFor(x => x.Rol)
            .Must(rol => rol == Roles.Reclutador || rol == Roles.Experto)
            .WithMessage("El rol debe ser Reclutador o Experto.");
    }
}