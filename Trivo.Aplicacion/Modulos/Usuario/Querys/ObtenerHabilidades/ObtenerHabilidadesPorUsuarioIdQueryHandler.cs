using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using Trivo.Aplicacion.Abstracciones.Mensajes;
using Trivo.Aplicacion.DTOs.Habilidades;
using Trivo.Aplicacion.Interfaces.Repositorio.Cuenta;
using Trivo.Aplicacion.Utilidades;

namespace Trivo.Aplicacion.Modulos.Usuario.Querys.ObtenerHabilidades;

internal sealed class ObtenerHabilidadesPorUsuarioIdQueryHandler(
    ILogger<ObtenerHabilidadesPorUsuarioIdQueryHandler> logger,
    IRepositorioUsuario repositorioUsuario,
    IDistributedCache cache
    ) : IQueryHandler<ObtenerHabilidadesPorUsuarioIdQuery, IEnumerable<HabilidadConIdDto>>
{
    public async Task<ResultadoT<IEnumerable<HabilidadConIdDto>>> Handle(ObtenerHabilidadesPorUsuarioIdQuery request, CancellationToken cancellationToken)
    {
        var usuario = await repositorioUsuario.ObtenerByIdAsync(request.UsuarioId, cancellationToken);
        if (usuario is null)
        {
            logger.LogWarning("No se encontró el usuario para obtener su foto de perfil.");
    
            return ResultadoT<IEnumerable<HabilidadConIdDto>>.Fallo(Error.NoEncontrado("404", "El usuario no existe"));
        }
        
        var usuarioHabilidadesDto = await cache.ObtenerOCrearAsync($"obtener-habilidades-por-usuario-id-{request.UsuarioId}",
            async () =>
            {
                var obtenerHabilidadesPorUsuarioIdAsync =  await repositorioUsuario.ObtenerHabilidadesPorUsuarioIdAsync(request.UsuarioId, cancellationToken);
                return obtenerHabilidadesPorUsuarioIdAsync.Select(x => new HabilidadConIdDto
                (
                    HabilidadId: x.HabilidadId ?? Guid.Empty,
                    Nombre: x.Habilidad!.Nombre
                )).ToList();
            },
            cancellationToken: cancellationToken);
        
        if (!usuarioHabilidadesDto.Any())
        {
            logger.LogInformation("El usuario con ID {UsuarioId} no tiene habilidades registradas.", request.UsuarioId);

            return ResultadoT<IEnumerable<HabilidadConIdDto>>.Fallo(
                Error.Fallo("404", "El usuario no tiene habilidades registradas.")
            );
        }
        
        logger.LogInformation("Se obtuvieron {Cantidad} habilidades del usuario con ID {UsuarioId}.", usuarioHabilidadesDto.Count, request.UsuarioId);

        return ResultadoT<IEnumerable<HabilidadConIdDto>>.Exito(usuarioHabilidadesDto);

    }
}