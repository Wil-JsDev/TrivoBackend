using FluentValidation;

namespace Trivo.Aplicacion.Modulos.Usuario.Commands.ConfirmarUsuario;

public class ConfirmarCuentaUsuarioValidacion : AbstractValidator<ConfirmarUsuarioCommand>
{
    public ConfirmarCuentaUsuarioValidacion()
    {
        RuleFor(x => x.UsuarioId)
            .NotEmpty()
            .WithMessage("El ID del usuario no puede estar vacío.")
            .Must(id => id != Guid.Empty)
            .WithMessage("El ID del usuario no puede ser un GUID vacío.");

        RuleFor(x => x.Codigo)
            .NotEmpty()
            .WithMessage("El código de confirmación es obligatorio.")
            .Matches(@"^\d{6}$")
            .WithMessage("El código de confirmación debe contener exactamente 6 dígitos numéricos.");
    }
}