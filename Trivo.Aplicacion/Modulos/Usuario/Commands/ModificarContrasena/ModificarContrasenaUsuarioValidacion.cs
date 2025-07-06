using FluentValidation;

namespace Trivo.Aplicacion.Modulos.Usuario.Commands.ModificarContrasena;

public class ModificarContrasenaUsuarioValidacion : AbstractValidator<ModificarContrasenaUsuarioCommand>
{
    public ModificarContrasenaUsuarioValidacion()
    {
        RuleFor(x => x.UsuarioId)
            .NotEmpty()
            .WithMessage("El ID del usuario es obligatorio.");

        RuleFor(x => x.Contrasena)
            .NotEmpty()
            .WithMessage("La nueva contraseña es obligatoria.")
            .MinimumLength(6)
            .WithMessage("La contraseña debe tener al menos 6 caracteres.")
            .MaximumLength(30)
            .WithMessage("La contraseña no debe exceder los 30 caracteres.");

        RuleFor(x => x.ConfirmacionDeContrsena)
            .NotEmpty()
            .WithMessage("Debe confirmar la contraseña.")
            .Equal(x => x.Contrasena)
            .WithMessage("La confirmación de la contraseña no coincide.");

        RuleFor(x => x.Codigo)
            .NotEmpty()
            .WithMessage("El codigo de confirmación es obligatorio.");

    }
}