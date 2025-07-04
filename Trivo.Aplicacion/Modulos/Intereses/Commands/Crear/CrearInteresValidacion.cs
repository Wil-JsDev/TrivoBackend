using FluentValidation;
using Trivo.Aplicacion.Interfaces.Repositorio;

namespace Trivo.Aplicacion.Modulos.Intereses.Commands.Crear;

public class CrearInteresValidacion : AbstractValidator<CrearInteresCommand>
{
    public CrearInteresValidacion(IRepositorioInteres repositorioInteres)
    {
        RuleFor(x => x.Nombre)
            .NotEmpty()
            .WithMessage("El nombre no puede ser nulo");
        
        RuleFor(x => x.CategoriaId)
            .NotNull()
            .WithMessage("Debe especificar una categoría válida");

        RuleFor(x => x.CreadoPor)
            .NotNull()
            .WithMessage("Debe especificar el usuario que crea el interés");
        
    }
}