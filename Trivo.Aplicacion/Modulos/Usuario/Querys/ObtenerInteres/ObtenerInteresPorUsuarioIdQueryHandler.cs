using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using Trivo.Aplicacion.Abstracciones.Mensajes;
using Trivo.Aplicacion.DTOs.Intereses;
using Trivo.Aplicacion.Interfaces.Repositorio.Cuenta;
using Trivo.Aplicacion.Utilidades;

namespace Trivo.Aplicacion.Modulos.Usuario.Querys.ObtenerInteres;

internal sealed class ObtenerInteresPorUsuarioIdQueryHandler(
    ILogger<ObtenerInteresPorUsuarioIdQueryHandler> logger,
    IRepositorioUsuario repositorioUsuario,
    IDistributedCache cache
    ) : IQueryHandler<ObtenerInteresPorUsuarioIdQuery, IEnumerable<InteresDto>>
{
    public async Task<ResultadoT<IEnumerable<InteresDto>>> Handle(ObtenerInteresPorUsuarioIdQuery request, CancellationToken cancellationToken)
    {
        var usuario = await repositorioUsuario.ObtenerByIdAsync(request.UsuarioId, cancellationToken);
        if (usuario is null)
        {
            logger.LogWarning("No se encontró el usuario para obtener su foto de perfil.");
    
            return ResultadoT<IEnumerable<InteresDto>>.Fallo(Error.NoEncontrado("404", "El usuario no existe"));
        }

        var usuarioInteresDto = await cache.ObtenerOCrearAsync($"obtener-intereses-por-usuario-id-{request.UsuarioId}",
            async () =>
            {
                var usuarioInteres = await repositorioUsuario.ObtenerInteresesPorUsuarioIdAsync(request.UsuarioId, cancellationToken);
                return usuarioInteres.Select(x => new InteresDto
                (
                    InteresId: x.InteresId ?? Guid.Empty,
                    Nombre: x.Interes!.Nombre!
                ));
            },
            cancellationToken: cancellationToken
        );


        IEnumerable<InteresDto> interesDtos = usuarioInteresDto.ToList();
        if (!interesDtos.Any())
        {
            logger.LogWarning("El usuario con ID {UsuarioId} no tiene intereses registrados.", request.UsuarioId);

            return ResultadoT<IEnumerable<InteresDto>>.Fallo(
                Error.Fallo("usuario_sin_intereses", "El usuario no tiene intereses registrados.")
            );
        }
        

        logger.LogInformation("Se obtuvieron {Cantidad} intereses del usuario con ID {UsuarioId}.", interesDtos.Count(), request.UsuarioId);

        return ResultadoT<IEnumerable<InteresDto>>.Exito(interesDtos);

    }
}