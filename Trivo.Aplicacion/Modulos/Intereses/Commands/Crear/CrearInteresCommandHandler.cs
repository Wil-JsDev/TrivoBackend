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
            logger.LogWarning("No se puede crear el Interes");
            
            return ResultadoT<InteresDetallesDto>.Fallo(Error.Fallo("400","No se puede crear el Interes"));
        }
        
        var categoriaId = await repositorioCategoriaInteres.ObtenerPorIdAsync(request.CategoriaId ?? Guid.Empty, cancellationToken);
        if (categoriaId is null)
        {
            logger.LogWarning("No se puede crear el Interes");
            
            return ResultadoT<InteresDetallesDto>.Fallo(Error.NoEncontrado("404","No se puede crear el Interes"));
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