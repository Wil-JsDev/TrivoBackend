using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using Trivo.Aplicacion.Abstracciones.Mensajes;
using Trivo.Aplicacion.DTOs.Intereses;
using Trivo.Aplicacion.Interfaces.Repositorio;
using Trivo.Aplicacion.Utilidades;

namespace Trivo.Aplicacion.Modulos.Intereses.Querys.BuscarPorNombre;

internal sealed class BuscarInteresesPorNombreQueryHandler(
    ILogger<BuscarInteresesPorNombreQueryHandler> logger,
    IRepositorioInteres repositorioInteres,
    IDistributedCache cache
    ) : IQueryHandler<BuscarInteresesPorNombreQuery, IEnumerable<InteresConIdDto>>
{
    public async Task<ResultadoT<IEnumerable<InteresConIdDto>>> Handle(BuscarInteresesPorNombreQuery request, CancellationToken cancellationToken)
    {
        
        if (string.IsNullOrWhiteSpace(request.Nombre))
        {
            logger.LogWarning("El nombre ingresado para la búsqueda de intereses está vacío o en blanco.");
            
            return ResultadoT<IEnumerable<InteresConIdDto>>.Fallo(
                Error.Fallo("400", "El nombre de interés no puede estar vacío."));
        }

        // ToLowerInvariant() garantiza que la clave de la cache sea consistente sin importar como lo escribio el usuario
        var cacheLlave = $"buscar-interes-por-nombre-{request.Nombre.Trim().ToLowerInvariant()}";

        var interesesConNombre = await cache.ObtenerOCrearAsync(cacheLlave,
            async () => await repositorioInteres.BuscarInteresesPorNombreAsync(request.Nombre, cancellationToken),
            cancellationToken: cancellationToken);

        if (!interesesConNombre.Any())
        {
            logger.LogInformation("No se encontraron intereses que coincidan con el texto: '{Nombre}'.", request.Nombre);
            return ResultadoT<IEnumerable<InteresConIdDto>>.Fallo(
                Error.Fallo("404", "No se encontraron intereses que coincidan con el nombre ingresado."));
        }

        var interesesDto = interesesConNombre.Select(x => new InteresConIdDto
        (
            InteresId: x.Id ?? Guid.Empty,
            Nombre: x.Nombre ?? string.Empty
        )).ToList();

        logger.LogInformation("Se encontraron {Cantidad} intereses que coinciden con '{Nombre}'.", 
            interesesDto.Count, request.Nombre);

        return ResultadoT<IEnumerable<InteresConIdDto>>.Exito(interesesDto);
    }
}