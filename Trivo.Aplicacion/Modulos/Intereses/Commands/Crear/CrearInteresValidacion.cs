using FluentValidation;
using Trivo.Aplicacion.Interfaces.Repositorio;

namespace Trivo.Aplicacion.Modulos.Intereses.Commands.Crear;

public class CrearInteresValidacion : AbstractValidator<CrearInteresCommand>
{
    public CrearInteresValidacion(IRepositorioInteres repositorioInteres)
    {
        RuleFor(x => x.Nombre)
            .NotEmpty()
            .WithMessage("El nombre no puede ser nulo")
            .MustAsync(async (nombre, ct) => !await repositorioInteres.NombreExisteAsync(nombre, ct))
            .WithMessage("Este nombre ya está registrado");
        
        RuleFor(x => x.CategoriaId)
            .NotNull()
            .WithMessage("Debe especificar una categoría válida");

        RuleFor(x => x.CreadoPor)
            .NotNull()
            .WithMessage("Debe especificar el usuario que crea el interés");
        
        // Validación compuesta: mismo nombre en la misma categoría no puede existir
        RuleFor(x => x)
            .MustAsync(async (command, ct) =>
                !await repositorioInteres.NombreCategoriaExisteAsync(command.Nombre, command.CategoriaId ?? Guid.Empty, ct))
            .WithMessage("Este interés ya existe en la categoría especificada");
        
    }
}