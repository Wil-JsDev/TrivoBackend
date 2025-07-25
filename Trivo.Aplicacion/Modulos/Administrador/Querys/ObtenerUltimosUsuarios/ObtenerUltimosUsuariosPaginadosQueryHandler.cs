using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using Trivo.Aplicacion.Abstracciones.Mensajes;
using Trivo.Aplicacion.DTOs.Cuentas.Usuarios;
using Trivo.Aplicacion.Interfaces.Repositorio.Cuenta;
using Trivo.Aplicacion.Mapper;
using Trivo.Aplicacion.Paginacion;
using Trivo.Aplicacion.Utilidades;

namespace Trivo.Aplicacion.Modulos.Administrador.Querys.ObtenerUltimosUsuarios;

internal sealed class ObtenerUltimosUsuariosPaginadosQueryHandler(
    ILogger<ObtenerUltimosUsuariosPaginadosQueryHandler> logger,
    IRepositorioAdministrador repositorioAdministrador,
    IDistributedCache cache
    ) : IQueryHandler<ObtenerUltimosUsuariosPaginadosQuery, ResultadoPaginado<UsuarioDto>>
{
    public async Task<ResultadoT<ResultadoPaginado<UsuarioDto>>> Handle(ObtenerUltimosUsuariosPaginadosQuery request, CancellationToken cancellationToken)
    {
        if (request is null)
        {
            logger.LogWarning("La solicitud para obtener los últimos usuarios registrados fue nula.");
        
            return ResultadoT<ResultadoPaginado<UsuarioDto>>.Fallo(
                Error.Fallo("400", "La solicitud no puede ser nula."));
        }

        if (request.NumeroPagina <= 0 || request.TamanoPagina <= 0)
        {
            logger.LogWarning("Parámetros inválidos para la paginación: Número de página ({NumeroPagina}) o Tamaño de página ({TamanoPagina}) no son válidos.", 
                request.NumeroPagina, request.TamanoPagina);
        
            return ResultadoT<ResultadoPaginado<UsuarioDto>>.Fallo(
                Error.Fallo("400", "Número de página y tamaño de página deben ser mayores a cero."));
        }

        var resultadoPaginadoDto = await cache.ObtenerOCrearAsync(
            $"obtener-ultimos-usuarios-registrados-{request.NumeroPagina}-{request.TamanoPagina}",
            async () =>
            {
                var resultado = await repositorioAdministrador.ObtenerPaginadoUltimosUsuariosAsync(request.NumeroPagina,
                    request.TamanoPagina,
                    cancellationToken);
                
                var resultadoDto = resultado.Elementos!
                    .Select(UsuarioMapper.MapUsuarioDto)
                    .ToList();
                
                var resultadoPaginado = new ResultadoPaginado<UsuarioDto>(
                    elementos: resultadoDto,
                    totalElementos: resultado.TotalElementos,
                    paginaActual: request.NumeroPagina,
                    tamanioPagina: request.TamanoPagina
                );
                
                return resultadoPaginado;
            },
            cancellationToken: cancellationToken
        );
        
        logger.LogInformation("Se obtuvieron {Cantidad} usuarios registrados recientemente (Página: {NumeroPagina}, Tamaño: {TamanoPagina}).",
            resultadoPaginadoDto.Elementos!.Count(), request.NumeroPagina, request.TamanoPagina);

        
        return ResultadoT<ResultadoPaginado<UsuarioDto>>.Exito(resultadoPaginadoDto);
    }
}