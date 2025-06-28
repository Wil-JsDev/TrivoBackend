using FluentValidation;
using Trivo.Aplicacion.Interfaces.Repositorio;

namespace Trivo.Aplicacion.Modulos.Habilidades.Commands.Crear;

public class CrearHabilidadValidacion : AbstractValidator<CrearHabilidadCommand>
{
    public CrearHabilidadValidacion(IRepositorioHabilidad repositorioHabilidad)
    {
        RuleFor(x => x.Nombre)
            .NotEmpty()
            .WithMessage("El nombre no puede ser nulo")
            .MustAsync(async (nombre, ct) => !await repositorioHabilidad.NombreExisteAsync(nombre, ct))
            .WithMessage("Este nombre ya está registrado.");
        
        RuleFor(x => x.UsearioId)
            .NotEmpty()
            .WithMessage("El id del usuario no puede ser nulo");

        RuleFor(x => x.Nivel)
            .NotEmpty()
            .WithMessage("El nivel no puede ser nulo");
    }
}