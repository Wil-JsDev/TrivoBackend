using FluentValidation;

namespace Trivo.Aplicacion.Modulos.Usuario.Commands.ActualizarBiografia;

public class ActualizarUsuarioBiografiaValidacion : AbstractValidator<ActualizarUsuarioBiografiaCommand>
{
    public ActualizarUsuarioBiografiaValidacion()
    {
        RuleFor(x => x.UsuarioId)
            .NotEmpty().WithMessage("El Id del usuario es obligatorio");
        
        RuleFor(x => x.Biografia)
            .NotEmpty().WithMessage("La biografía es obligatoria");
    }    
}