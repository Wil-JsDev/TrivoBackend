using FluentValidation;

namespace Trivo.Aplicacion.Modulos.Administrador.Commands.BanearUsuarios;

public class BanearUsuariosValidacion : AbstractValidator<BanearUsuariosCommand>
{
    public BanearUsuariosValidacion()
    {
        RuleFor(x => x.UsuarioId)
            .NotNull().WithMessage("El ID del usuario es obligatorio.")
            .NotEqual(Guid.Empty)
            .WithMessage("El ID del usuario no puede ser un GUID vacío.");
    }
}