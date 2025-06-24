using FluentValidation;

namespace Trivo.Aplicacion.Modulos.Usuario.Commands.Actualizar;

public class ActualizarUsuarioValidacion : AbstractValidator<ActualizarUsuarioCommand>
{
    public ActualizarUsuarioValidacion()
    {
        RuleFor(x => x.UsuarioId)
            .NotEmpty()
            .WithMessage("El ID del usuario es obligatorio");

        RuleFor(x => x.NombreUsuario)
            .NotEmpty().WithMessage("El nombre de usuario es obligatorio")
            .MaximumLength(50).WithMessage("El nombre de usuario no debe exceder los 50 caracteres");

        RuleFor(x => x.Email)
            .NotEmpty()
            .WithMessage("El correo electrónico es obligatorio")
            .EmailAddress()
            .WithMessage("Debe proporcionar un correo electrónico válido");
    }
}