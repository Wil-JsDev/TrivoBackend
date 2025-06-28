using FluentValidation;
using Trivo.Aplicacion.Interfaces.Repositorio;

namespace Trivo.Aplicacion.Modulos.CategoriaIntereses.Commands;

public class CrearCategoriaInteresValidacion : AbstractValidator<CrearCategoriaInteresCommand>
{
    public CrearCategoriaInteresValidacion(IRepositorioCategoriaInteres repositorio)
    {

        RuleFor(x => x.Nombre)
            .NotEmpty()
            .WithMessage("El nombre no puede ser nulo")
            .MustAsync(async (nombre, ct) => !await repositorio.NombreExisteAsync(nombre, ct))
            .WithMessage("Este nombre ya está registrado.");

    }    
}