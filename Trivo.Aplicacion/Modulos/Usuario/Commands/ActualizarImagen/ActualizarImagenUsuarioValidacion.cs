using FluentValidation;

namespace Trivo.Aplicacion.Modulos.Usuario.Commands.ActualizarImagen;

public class ActualizarImagenUsuarioValidacion : AbstractValidator<ActualizarImagenUsuarioCommand>
{
    private readonly string[] _extensionesPermitidas = [".jpg", ".jpeg", ".png", ".webp"];
    
    public ActualizarImagenUsuarioValidacion()
    {
         RuleFor(x => x.UsuarioId)
            .NotEmpty()
            .WithMessage("El ID del usuario es obligatorio");

        RuleFor(x => x.Imagen)
            .NotNull()
            .WithMessage("La imagen es obligatoria.")
            .Must(file => file!.Length > 0)
            .WithMessage("La imagen no puede estar vacía.")
            .Must(file => _extensionesPermitidas.Contains(Path.GetExtension(file.FileName).ToLower()))
            .WithMessage("Solo se permiten imágenes con formato .jpg, .jpeg, .png o .webp.");
    }
}