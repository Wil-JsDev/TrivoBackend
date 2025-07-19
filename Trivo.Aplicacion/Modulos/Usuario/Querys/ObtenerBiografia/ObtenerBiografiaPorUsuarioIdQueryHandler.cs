using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using Trivo.Aplicacion.Abstracciones.Mensajes;
using Trivo.Aplicacion.DTOs.Cuentas.Usuarios;
using Trivo.Aplicacion.Interfaces.Repositorio.Cuenta;
using Trivo.Aplicacion.Utilidades;

namespace Trivo.Aplicacion.Modulos.Usuario.Querys.ObtenerBiografia;

internal sealed class ObtenerBiografiaPorUsuarioIdQueryHandler(
    ILogger<ObtenerBiografiaPorUsuarioIdQueryHandler> logger,
    IRepositorioUsuario repositorioUsuario,
    IDistributedCache cache   
    ) : IQueryHandler<ObtenerBiografiaPorUsuarioIdQuery, BiografiaUsuarioDto>
{
    public async Task<ResultadoT<BiografiaUsuarioDto>> Handle(ObtenerBiografiaPorUsuarioIdQuery request, CancellationToken cancellationToken)
    {
        var usuario = await repositorioUsuario.ObtenerByIdAsync(request.UsuarioId, cancellationToken);
        
        if (usuario is null)
        {
            logger.LogWarning("No se encontró el usuario para obtener su foto de perfil.");
    
            return ResultadoT<BiografiaUsuarioDto>.Fallo(Error.NoEncontrado("404", "El usuario no existe"));
        }
        
        return ResultadoT<BiografiaUsuarioDto>.Exito(new BiografiaUsuarioDto(usuario.Biografia));
    }
}