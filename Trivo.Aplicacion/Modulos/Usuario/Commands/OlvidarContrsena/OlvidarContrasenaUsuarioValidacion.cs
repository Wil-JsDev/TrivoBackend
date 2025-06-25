using FluentValidation;

namespace Trivo.Aplicacion.Modulos.Usuario.Commands.OlvidarContrsena;

public class OlvidarContrasenaUsuarioValidacion : AbstractValidator<OlvidarContrasenaUsuarioCommand>
{
    public OlvidarContrasenaUsuarioValidacion()
    {
        RuleFor(x => x.UsuarioId)
            .NotEmpty()
            .WithMessage("El ID del usuario es obligatorio");
    }
}
