using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using Trivo.Aplicacion.Abstracciones.Mensajes;
using Trivo.Aplicacion.DTOs.Cuentas.Usuarios;
using Trivo.Aplicacion.Interfaces.Repositorio.Cuenta;
using Trivo.Aplicacion.Utilidades;

namespace Trivo.Aplicacion.Modulos.Usuario.Querys.ObtenerFotoDePerfil;

internal sealed class ObtenerFotoPerfilPorUsuarioIdQueryHandler(
    ILogger<ObtenerFotoPerfilPorUsuarioIdQuery> logger,
    IRepositorioUsuario repositorioUsuario,
    IDistributedCache cache
    ) : IQueryHandler<ObtenerFotoPerfilPorUsuarioIdQuery, FotoDePerfilUsuarioDto>
{
    public async Task<ResultadoT<FotoDePerfilUsuarioDto>> Handle(ObtenerFotoPerfilPorUsuarioIdQuery request, CancellationToken cancellationToken)
    {
        // var usuario = await cache.ObtenerOCrearAsync($"obtener-foto-perfil-por-usuario-id-{request.UsuarioId}", 
        //     async () => await repositorioUsuario.ObtenerByIdAsync(request.UsuarioId, cancellationToken), 
        //     cancellationToken: cancellationToken);
        
        var usuario = await repositorioUsuario.ObtenerByIdAsync(request.UsuarioId, cancellationToken);
        
        if (usuario is null)
        {
            logger.LogWarning("No se encontró el usuario para obtener su foto de perfil.");
    
            return ResultadoT<FotoDePerfilUsuarioDto>.Fallo(Error.NoEncontrado("404", "El usuario no existe"));
        }

        
        logger.LogInformation("Se obtuvo correctamente la foto de perfil del usuario con ID '{UsuarioId}'.", usuario.Id);
        
        return ResultadoT<FotoDePerfilUsuarioDto>.Exito(new FotoDePerfilUsuarioDto(usuario.FotoPerfil));
    }
}