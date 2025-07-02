using FluentValidation;

namespace Trivo.Aplicacion.Modulos.Usuario.Commands.InicioSesion;

public class InicioSesionUsuarioValidacion : AbstractValidator<InicioSesionUsuarioCommand>
{
    public InicioSesionUsuarioValidacion()
    {
        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("El correo electrónico es obligatorio")
            .EmailAddress().WithMessage("Debe proporcionar un correo electrónico válido")
            .MaximumLength(100).WithMessage("El correo electrónico no debe exceder los 100 caracteres");
        
        RuleFor(x => x.Contrasena)
            .NotEmpty().WithMessage("La contraseña es obligatoria")
            .MinimumLength(6).WithMessage("La contraseña debe tener al menos 6 caracteres")
            .MaximumLength(30).WithMessage("La contraseña no debe exceder los 30 caracteres");
    }
}