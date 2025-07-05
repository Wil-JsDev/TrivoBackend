using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using Trivo.Aplicacion.Abstracciones.Mensajes;
using Trivo.Aplicacion.DTOs.Emparejamiento;
using Trivo.Aplicacion.Interfaces.Repositorio;
using Trivo.Aplicacion.Interfaces.Repositorio.Cuenta;
using Trivo.Aplicacion.Utilidades;
using Trivo.Dominio.Enum;

namespace Trivo.Aplicacion.Modulos.Emparejamiento.Querys;

internal sealed class ObtenerEmparejamientoPorUsuarioQueryHandler(
    ILogger<ObtenerEmparejamientoPorUsuarioQueryHandler> logger,
    IRepositorioEmparejamiento repositorioEmparejamiento,
    IRepositorioUsuario repositorioUsuario,
    IDistributedCache cache
    ) : IQueryHandler<ObtenerEmparejamientoPorUsuarioQuery, IEnumerable<EmparejamientoDto>>
{
    public async Task<ResultadoT<IEnumerable<EmparejamientoDto>>> Handle(ObtenerEmparejamientoPorUsuarioQuery request, CancellationToken cancellationToken)
    {
        if (request is null)
        {
            logger.LogWarning("La solicitud de obtención de emparejamientos es nula.");
            return ResultadoT<IEnumerable<EmparejamientoDto>>.Fallo(Error.Fallo("400", "La solicitud no puede ser nula."));
        }

        var usuario = await repositorioUsuario.ObtenerByIdAsync(request.UsuarioId, cancellationToken);
        if (usuario is null)
        {
            logger.LogWarning("No se encontró el usuario con ID {UsuarioId}.", request.UsuarioId);
            return ResultadoT<IEnumerable<EmparejamientoDto>>.Fallo(Error.NoEncontrado("404", "El usuario no existe"));
        }

        var estrategiasEmparejamiento = Obtener(request.UsuarioId);

        if (!estrategiasEmparejamiento.TryGetValue(request.Rol, out var filtroEmparejamiento))
        {
            logger.LogWarning("El rol proporcionado '{Rol}' no tiene una estrategia de emparejamiento válida.", request.Rol);
            
            return ResultadoT<IEnumerable<EmparejamientoDto>>.Fallo(Error.Fallo("400", "El rol proporcionado no es válido para emparejamientos."));
        }

        var emparejamientos = await cache.ObtenerOCrearAsync(
            $"obtener-emparejamiento-por-usuario-{request.UsuarioId}-{request.Rol}",
            async () => await filtroEmparejamiento(cancellationToken),
            cancellationToken: cancellationToken
        );

        var emparejamientosLista = emparejamientos.ToList();

        if (!emparejamientosLista.Any())
        {
            logger.LogWarning("No se encontraron emparejamientos pendientes para el usuario {UsuarioId} con el rol {Rol}.", request.UsuarioId, request.Rol);
            
            return ResultadoT<IEnumerable<EmparejamientoDto>>.Fallo(Error.Fallo("404", "No se encontraron emparejamientos para el usuario."));
        }

        var emparejamientoDto = emparejamientosLista.Select(x => new EmparejamientoDto
        (
            EmparejamientoId: x.Id!.Value,
            ReclutadorId: x.ReclutadorId!.Value,
            ExpertoId: x.ExpertoId!.Value,
            EstadoExperto: x.ExpertoEstado!,
            EstadoReclutador: x.ReclutadorEstado!,
            EstadoEmparejamiento: x.EmparejamientoEstado!
        ));

        logger.LogInformation("Se recuperaron correctamente {Cantidad} emparejamientos para el usuario {UsuarioId} con rol {Rol}.",
            emparejamientosLista.Count, request.UsuarioId, request.Rol);

        return ResultadoT<IEnumerable<EmparejamientoDto>>.Exito(emparejamientoDto);
    }
    
    #region Privados

    private Dictionary<Roles, Func<CancellationToken, Task<IEnumerable<Dominio.Modelos.Emparejamiento>>>> Obtener(Guid usuarioId)
    {
        return new Dictionary<Roles, Func<CancellationToken, Task<IEnumerable<Dominio.Modelos.Emparejamiento>>>>
        {
            { Roles.Experto, async cancellationToken => await repositorioEmparejamiento.ObtenerEmparejamientosComoExpertoAsync(usuarioId,cancellationToken) },
            { Roles.Reclutador, async cancellationToken => await repositorioEmparejamiento.ObtenerEmparejamientosComoReclutadorAsync(usuarioId,cancellationToken)}
        };
    }

    #endregion
}