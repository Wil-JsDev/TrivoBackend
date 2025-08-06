using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using Trivo.Aplicacion.Abstracciones.Mensajes;
using Trivo.Aplicacion.DTOs.Cuentas.Administrador;
using Trivo.Aplicacion.Interfaces.Repositorio.Cuenta;
using Trivo.Aplicacion.Paginacion;
using Trivo.Aplicacion.Utilidades;

namespace Trivo.Aplicacion.Modulos.Administrador.Querys.ObtenerUltimosEmparejamientos;

internal sealed class ObtenerUltimosEmparejamientosQueryHandler(
    ILogger<ObtenerUltimosEmparejamientosQueryHandler> logger,
    IRepositorioAdministrador repositorioAdministrador,
    IDistributedCache cache
    ) : IQueryHandler<ObtenerUltimosEmparejamientosQuery, ResultadoPaginado<EmparejamientoAdministradorDto>>
{
    public async Task<ResultadoT<ResultadoPaginado<EmparejamientoAdministradorDto>>> Handle(ObtenerUltimosEmparejamientosQuery request, CancellationToken cancellationToken)
    {
       
        if (request.NumeroPagina <= 0 || request.TamanoPagina <= 0)
        {
            logger.LogWarning("Paginación inválida: número de página ({NumeroPagina}) o tamaño de página ({TamanoPagina}) no son mayores que cero.",
                request.NumeroPagina, request.TamanoPagina);

            return ResultadoT<ResultadoPaginado<EmparejamientoAdministradorDto>>.Fallo(
                Error.Fallo("400", "La paginación es inválida. Ambos valores deben ser mayores que cero."));
        }

        var emparejamientosResultado =
            await repositorioAdministrador.ObtenerPaginadoUltimosEmparejamientosAsync(request.NumeroPagina,
                request.TamanoPagina, cancellationToken);

        if (!emparejamientosResultado.Elementos!.Any())
        {
            logger.LogWarning("No se encontraron emparejamientos en la página {NumeroPagina} con tamaño {TamanoPagina}.",
                request.NumeroPagina, request.TamanoPagina);

            return ResultadoT<ResultadoPaginado<EmparejamientoAdministradorDto>>.Fallo(
                Error.Fallo("400", "No se encontraron emparejamientos para la página solicitada."));
        }

        var emparejamientosPaginados = await cache.ObtenerOCrearAsync(
            $"obtener-ultimos-emparejamientos-{request.NumeroPagina}-{request.TamanoPagina}",
            async () =>
            {
                var resultadoPaginadDto = emparejamientosResultado.Elementos!.Select(x =>
                    new EmparejamientoAdministradorDto
                    (
                        EmparejamientoId: x.Id ?? Guid.Empty,
                        ReclutadotId: x.ReclutadorId ?? Guid.Empty,
                        ExpertoId: x.ExpertoId ?? Guid.Empty,
                        ExpertoEstado: x.ExpertoEstado,
                        ReclutadorEstado: x.ReclutadorEstado,
                        EmparejamientoEstado: x.EmparejamientoEstado,
                        FechaRegistro: x.FechaRegistro,
                        ReclutadorDto: new ReclutadorEmparejamientoDto
                        (
                            Nombre: x.Reclutador!.Usuario!.Nombre,
                            Apellido: x.Reclutador.Usuario.Apellido! 
                        ),
                        ExpertoDto: new ExpertoEmparejamientoDto
                        (
                            Nombre: x.Experto!.Usuario!.Nombre!,
                            Apellido: x.Experto.Usuario.Apellido! 
                        )
                    )).ToList();

                var total = resultadoPaginadDto.Count();
                
                ResultadoPaginado<EmparejamientoAdministradorDto> resultadoPaginado = new
                (
                    elementos: resultadoPaginadDto,
                    totalElementos: total,
                    paginaActual: request.NumeroPagina,
                    request.TamanoPagina
                );

                return resultadoPaginado;
            },
            cancellationToken: cancellationToken
        );

        logger.LogInformation("Emparejamientos obtenidos correctamente para la página {NumeroPagina} con tamaño {TamanoPagina}.",
            request.NumeroPagina, request.TamanoPagina);

        return emparejamientosPaginados;
    }
}