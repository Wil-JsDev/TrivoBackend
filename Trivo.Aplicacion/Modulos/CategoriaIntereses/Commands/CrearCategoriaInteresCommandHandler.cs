using Microsoft.Extensions.Logging;
using Trivo.Aplicacion.Abstracciones.Mensajes;
using Trivo.Aplicacion.DTOs.CategoriaIntereses;
using Trivo.Aplicacion.Interfaces.Repositorio;
using Trivo.Aplicacion.Utilidades;
using Trivo.Dominio.Modelos;

namespace Trivo.Aplicacion.Modulos.CategoriaIntereses.Commands;

internal sealed class CrearCategoriaInteresCommandHandler(
    ILogger<CrearCategoriaInteresCommandHandler> logger,
    IRepositorioCategoriaInteres categoriaInteresRepositorio
    ) : ICommandHandler<CrearCategoriaInteresCommand, CategoriaInteresDto>
{
    public async Task<ResultadoT<CategoriaInteresDto>> Handle(CrearCategoriaInteresCommand request, CancellationToken cancellationToken)
    {
        if (request is null)
        {
            logger.LogWarning("La solicitud para crear la categoría de interés es nula.");
    
            return ResultadoT<CategoriaInteresDto>.Fallo(Error.Fallo("400", "No se puede crear la categoría de interés. La solicitud es inválida."));
        }

        CategoriaInteres categoriaInteres = new()
        {
            CategoriaId = Guid.NewGuid(),
            Nombre = request.Nombre
        };

        await categoriaInteresRepositorio.CrearCategoriaInteresAsync(categoriaInteres, cancellationToken);

        logger.LogInformation("Se ha creado exitosamente la categoría de interés con ID {CategoriaId} y nombre '{Nombre}'.", 
            categoriaInteres.CategoriaId, categoriaInteres.Nombre);

        CategoriaInteresDto categoriaInteresDto = new
        (
            CategoriaInteresId: categoriaInteres.CategoriaId ?? Guid.Empty,
            Nombre: categoriaInteres.Nombre
        );

        return ResultadoT<CategoriaInteresDto>.Exito(categoriaInteresDto);

    }
}