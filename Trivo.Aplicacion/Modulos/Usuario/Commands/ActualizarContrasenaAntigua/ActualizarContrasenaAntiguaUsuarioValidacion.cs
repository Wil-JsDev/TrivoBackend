using FluentValidation;

namespace Trivo.Aplicacion.Modulos.Usuario.Commands.ActualizarContrasenaAntigua;

public class ActualizarContrasenaAntiguaUsuarioValidacion : AbstractValidator<ActualizarContrasenaAntiguaUsuarioCommand>
{
    public ActualizarContrasenaAntiguaUsuarioValidacion()
    {
        RuleFor(x => x.UsuarioId)
            .NotEmpty()
            .WithMessage("El Id del usuario es obligatorio.")
            .Must(id => id != Guid.Empty)
            .WithMessage("El Id del usuario no puede ser Guid.Empty.");

        RuleFor(x => x.ContrasenaAntigua)
            .NotEmpty()
            .WithMessage("La contraseña antigua es obligatoria.")
            .MinimumLength(6)
            .WithMessage("La contraseña antigua debe tener al menos 6 caracteres.")
            .MaximumLength(30)
            .WithMessage("La contraseña antigua no debe exceder los 30 caracteres.");
        
        RuleFor(x => x.NuevaContrasena)
            .NotEmpty()
            .WithMessage("La nueva contraseña es obligatoria.")
            .MinimumLength(6)
            .WithMessage("La contraseña debe tener al menos 6 caracteres.")
            .MaximumLength(30)
            .WithMessage("La contraseña no debe exceder los 30 caracteres.");

        RuleFor(x => x.ConfirmacionDeContrsena)
            .NotEmpty()
            .WithMessage("Debe confirmar la contraseña.")
            .Equal(x => x.NuevaContrasena)
            .WithMessage("La confirmación de la contraseña no coincide.");
    }
}