using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using Trivo.Aplicacion.Abstracciones.Mensajes;
using Trivo.Aplicacion.DTOs.Cuentas.Usuarios;
using Trivo.Aplicacion.DTOs.Reportes;
using Trivo.Aplicacion.Interfaces.Repositorio.Cuenta;
using Trivo.Aplicacion.Mapper;
using Trivo.Aplicacion.Paginacion;
using Trivo.Aplicacion.Utilidades;

namespace Trivo.Aplicacion.Modulos.Administrador.Querys.ObtenerUsuariosBaneados;

internal sealed class ObtenerUltimos10UsuariosBaneadosQueryHandler(
    ILogger<ObtenerUltimos10UsuariosBaneadosQueryHandler> logger,
    IRepositorioAdministrador repositorioAdministrador,
    IDistributedCache cache
    ) : IQueryHandler<ObtenerUltimos10UsuariosBaneadosQuery, IEnumerable<UsuarioDto>>
{
    public async Task<ResultadoT<IEnumerable<UsuarioDto>>> Handle(
        ObtenerUltimos10UsuariosBaneadosQuery request,
        CancellationToken cancellationToken)
    {

        var resultadoPaginado = await cache.ObtenerOCrearAsync(
            $"obtener-ultimos-usuarios-baneados",
            async () =>
            {
                var usuariosBaneados = await repositorioAdministrador.ObtenerUltimos10UsuariosBaneadosAsync(cancellationToken);
                
                var usuariosBaneadosDto = usuariosBaneados.Select(UsuarioMapper.MapUsuarioDto).ToList();
                
                return usuariosBaneadosDto;
            },
            cancellationToken: cancellationToken
        );

        if (!resultadoPaginado!.Any())
        {
            logger.LogWarning("No se encontraron usuarios baneados.");
            
            return ResultadoT<IEnumerable<UsuarioDto>>.Fallo(Error.Fallo("404", "No se encontraron usuarios baneados."));
        }
        
        logger.LogInformation("Se encontraron {Cantidad} usuarios baneados.", resultadoPaginado.Count);
        
        return ResultadoT<IEnumerable<UsuarioDto>>.Exito(resultadoPaginado);
    }

}