using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using Trivo.Aplicacion.Abstracciones.Mensajes;
using Trivo.Aplicacion.DTOs.Habilidades;
using Trivo.Aplicacion.Interfaces.Repositorio;
using Trivo.Aplicacion.Utilidades;

namespace Trivo.Aplicacion.Modulos.Habilidades.Querys.BuscarPorNombre;

internal sealed class BuscarHabilidadesPorNombreQueryHandler(
    ILogger<BuscarHabilidadesPorNombreQueryHandler> logger,
    IRepositorioHabilidad repositorioHabilidad,
    IDistributedCache cache
    ) : IQueryHandler<BuscarHabilidadesPorNombreQuery, IEnumerable<HabilidadConIdDto>>
{
    public async Task<ResultadoT<IEnumerable<HabilidadConIdDto>>> Handle(BuscarHabilidadesPorNombreQuery request, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(request.Nombre))
        {
            logger.LogWarning("El nombre ingresado para la búsqueda de habilidades está vacío o en blanco.");

            return ResultadoT<IEnumerable<HabilidadConIdDto>>.Fallo(
                Error.Fallo("400", "El nombre de la habilidad no puede estar vacío."));
        }

        var cacheLlave = $"buscar-habilidades-por-nombre-{request.Nombre.Trim().ToLowerInvariant()}";

        var resultadoHabilidades = await cache.ObtenerOCrearAsync(cacheLlave,
            async () => await repositorioHabilidad.BuscarHabilidadesPorNombreAsync(request.Nombre, cancellationToken),
            cancellationToken: cancellationToken
        );

        if (!resultadoHabilidades.Any())
        {
            logger.LogInformation("No se encontraron habilidades que coincidan con el nombre: '{Nombre}'.", request.Nombre);

            return ResultadoT<IEnumerable<HabilidadConIdDto>>.Fallo(
                Error.Fallo("404", "No se encontraron habilidades que coincidan con el nombre ingresado."));
        }

        var resultadoHabilidadesDto = resultadoHabilidades.Select(x => new HabilidadConIdDto
        (
            HabilidadId: x.HabilidadId ?? Guid.Empty,
            Nombre: x.Nombre!
        )).ToList();

        logger.LogInformation("Se encontraron {Cantidad} habilidades que coinciden con '{Nombre}'.",
            resultadoHabilidadesDto.Count,
            request.Nombre);
        
        return ResultadoT<IEnumerable<HabilidadConIdDto>>.Exito(resultadoHabilidadesDto);
    }

}