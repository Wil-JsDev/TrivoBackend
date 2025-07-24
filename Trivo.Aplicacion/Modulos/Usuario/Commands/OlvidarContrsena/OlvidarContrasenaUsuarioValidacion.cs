using FluentValidation;

namespace Trivo.Aplicacion.Modulos.Usuario.Commands.OlvidarContrsena;

public class OlvidarContrasenaUsuarioValidacion : AbstractValidator<OlvidarContrasenaUsuarioCommand>
{
    public OlvidarContrasenaUsuarioValidacion()
    {
        RuleFor(x => x.Email)
            .NotEmpty()
            .WithMessage("El correo electrónico es obligatorio.")
            .EmailAddress()
            .WithMessage("Debe proporcionar un correo electrónico válido.");
    }
}
