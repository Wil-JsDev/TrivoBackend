using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using Trivo.Aplicacion.Abstracciones.Mensajes;
using Trivo.Aplicacion.DTOs.Cuentas.Usuarios;
using Trivo.Aplicacion.DTOs.Habilidades;
using Trivo.Aplicacion.DTOs.Intereses;
using Trivo.Aplicacion.Interfaces.Repositorio.Cuenta;
using Trivo.Aplicacion.Utilidades;

namespace Trivo.Aplicacion.Modulos.Usuario.Querys.ObtenerDetalles;

internal sealed class ObtenerDetallesUsuarioQueryHandler(
    IRepositorioUsuario repositorioUsuario,
    ILogger<ObtenerDetallesUsuarioQueryHandler> logger,
    IDistributedCache cache
    ) : IQueryHandler<ObtenerDetallesUsuarioQuery,  UsuarioDetallesDto>
{
    public async Task<ResultadoT<UsuarioDetallesDto>> Handle(
        ObtenerDetallesUsuarioQuery request, 
        CancellationToken cancellationToken)
    {
        if (request == null)
        {
            logger.LogError("La solicitud para obtener detalles del usuario es nula.");
            
            return ResultadoT<UsuarioDetallesDto>.Fallo(Error.Fallo("400", "La solicitud no puede ser nula."));
        }

        var usuario = await repositorioUsuario.ObtenerByIdAsync(request.UsuarioId, cancellationToken);
        if (usuario == null)
        {
            logger.LogWarning("No se encontró el usuario con ID '{RequestUsuarioId}'.", request.UsuarioId);
            
            return ResultadoT<UsuarioDetallesDto>.Fallo(Error.NoEncontrado("404", "El usuario no fue encontrado."));
        }
        
        var usuarioDetalles = await cache.ObtenerOCrearAsync(
            $"obtener-detalles-usuario-{request.UsuarioId.ToString().ToLower()}",
            async () => await repositorioUsuario.ObtenerDetallesUsuarioPorIdAsync(request.UsuarioId, cancellationToken),
            cancellationToken: cancellationToken
        );
        
        UsuarioDetallesDto usuarioDetallesDto = new
        (
            Nombre: usuarioDetalles.Nombre,
            Apellido: usuarioDetalles.Apellido,
            Ubicacion: usuarioDetalles.Ubicacion,
            Biografia: usuarioDetalles.Biografia,
            FotoPerfil: usuarioDetalles.FotoPerfil,
            Habilidad: usuarioDetalles.UsuarioHabilidades?
                           .Select(x => new HabilidadDto(Nombre: x.Habilidad?.Nombre ?? string.Empty))
                       ?? [],
            Interes: usuarioDetalles.UsuarioInteres?
                         .Select(x => new InteresDto(Nombre: x.Interes?.Nombre ?? string.Empty))
                     ?? []
        );

        logger.LogInformation("Se obtuvieron correctamente los detalles del usuario con ID '{UsuarioId}'.", usuarioDetalles.Id);

        return ResultadoT<UsuarioDetallesDto>.Exito(usuarioDetallesDto);

    }
}