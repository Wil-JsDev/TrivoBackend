using FluentValidation;

namespace Trivo.Aplicacion.Modulos.Usuario.Commands.Crear;

public class CrearUsuarioValidacion : AbstractValidator<CrearUsuarioCommand>
{
    private readonly string[] _extensionesPermitidas = [".jpg", ".jpeg", ".png", ".webp"];

    public CrearUsuarioValidacion()
    {
        RuleFor(x => x.Nombre)
            .NotEmpty().WithMessage("El nombre es obligatorio.")
            .MaximumLength(30).WithMessage("El nombre no debe exceder los 30 caracteres.");

        RuleFor(x => x.Apellido)
            .NotEmpty().WithMessage("El apellido es obligatorio.")
            .MaximumLength(30).WithMessage("El apellido no debe exceder los 30 caracteres.");

        RuleFor(x => x.Biografia)
            .NotEmpty().WithMessage("La biografía es obligatoria");

        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("El correo electrónico es obligatorio")
            .EmailAddress().WithMessage("Debe proporcionar un correo electrónico válido")
            .MaximumLength(100).WithMessage("El correo electrónico no debe exceder los 100 caracteres");

        RuleFor(x => x.Contrasena)
            .NotEmpty().WithMessage("La contraseña es obligatoria")
            .MinimumLength(6).WithMessage("La contraseña debe tener al menos 6 caracteres")
            .MaximumLength(30).WithMessage("La contraseña no debe exceder los 30 caracteres");

        RuleFor(x => x.NombreUsuario)
            .NotEmpty().WithMessage("El nombre de usuario es obligatorio")
            .MaximumLength(50).WithMessage("El nombre de usuario no debe exceder los 50 caracteres");

        RuleFor(x => x.Ubicacion)
            .MaximumLength(50).WithMessage("La ubicación no debe exceder los 50 caracteres");

        When(x => x.Foto != null, () =>
        {
            RuleFor(x => x.Foto)
                .Must(f => f!.Length > 0).WithMessage("La imagen no puede estar vacía")
                .Must(f => _extensionesPermitidas.Contains(Path.GetExtension(f!.FileName).ToLower()))
                .WithMessage("Solo se permiten imágenes con formato .jpg, .jpeg, .png o .webp")
                .Must(f => f!.Length <= 5 * 1024 * 1024)
                .WithMessage("La imagen no debe superar los 5 MB");
        });
        
    }
}
