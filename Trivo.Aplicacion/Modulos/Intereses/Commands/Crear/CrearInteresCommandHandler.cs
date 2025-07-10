using Microsoft.Extensions.Logging;
using Trivo.Aplicacion.Abstracciones.Mensajes;
using Trivo.Aplicacion.DTOs.Intereses;
using Trivo.Aplicacion.Interfaces.Repositorio;
using Trivo.Aplicacion.Utilidades;
using Trivo.Dominio.Modelos;

namespace Trivo.Aplicacion.Modulos.Intereses.Commands.Crear;

internal sealed class CrearInteresCommandHandler(
    ILogger<CrearInteresCommandHandler> logger,
    IRepositorioCategoriaInteres repositorioCategoriaInteres,
    IRepositorioUsuarioInteres repositorioUsuarioInteres,
    IRepositorioInteres repositorioInteres
    ) : ICommandHandler<CrearInteresCommand, InteresDetallesDto>
{
    public async Task<ResultadoT<InteresDetallesDto>> Handle(CrearInteresCommand request, CancellationToken cancellationToken)
    {
        if (request is null)
        {
            logger.LogWarning("Solicitud de creación de interés nula. No se recibió información en el cuerpo de la petición.");

            return ResultadoT<InteresDetallesDto>.Fallo(
                Error.Fallo("400", "No se pudo procesar la solicitud: el cuerpo está vacío o mal formado.")
            );
        }

        var categoria = await repositorioCategoriaInteres.ObtenerPorIdAsync(request.CategoriaId ?? Guid.Empty, cancellationToken);
        if (categoria is null)
        {
            logger.LogWarning("No se encontró una categoría de interés con el ID proporcionado: {CategoriaId}", request.CategoriaId);

            return ResultadoT<InteresDetallesDto>.Fallo(
                Error.NoEncontrado("404", "La categoría de interés especificada no existe o el ID es inválido.")
            );
        }

        //Mismo nombre no puede existir en la categoria especificada
        if (await repositorioInteres.NombreCategoriaExisteAsync(request.Nombre, request.CategoriaId!.Value, cancellationToken))
        {
            logger.LogWarning("Este interes ya existe en la categoria especifica: {CategoriaId}", request.CategoriaId);;
            
            return ResultadoT<InteresDetallesDto>.Fallo(Error.Conflicto("409", "Este interés ya existe en la categoría especificada"));
        }

        Interes interesEntidad = new()
        {
            Id = Guid.NewGuid(),
            CategoriaId = request.CategoriaId,
            Nombre = request.Nombre,
            CreadoPor = request.CreadoPor
        };

        await repositorioInteres.CrearInteresAsync(interesEntidad, cancellationToken);

        InteresDetallesDto interesDetallesDto = new
        (
            InteresId:  interesEntidad.Id ?? Guid.Empty,
            Nombre: interesEntidad.Nombre,
            CategoriaId: interesEntidad.CategoriaId,
            CreadoPor: interesEntidad.CreadoPor ?? Guid.Empty
        );
        
        logger.LogInformation("Interes Creada");

        var usuarioInteres = new UsuarioInteres()
        {
            UsuarioId = request.CreadoPor,
            InteresId = interesEntidad.Id
        };
        
        await repositorioUsuarioInteres.CrearUsuarioInteresAsync(usuarioInteres, cancellationToken);

        return ResultadoT<InteresDetallesDto>.Exito(interesDetallesDto);
    }
}