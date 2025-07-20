using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using Trivo.Aplicacion.Abstracciones.Mensajes;
using Trivo.Aplicacion.DTOs.Cuentas.Usuarios;
using Trivo.Aplicacion.Interfaces.Repositorio;
using Trivo.Aplicacion.Interfaces.Repositorio.Cuenta;
using Trivo.Aplicacion.Mapper;
using Trivo.Aplicacion.Paginacion;
using Trivo.Aplicacion.Utilidades;

namespace Trivo.Aplicacion.Modulos.Usuario.Querys.ObtenerUsuariosPorHabilidadesInteres;

internal sealed class ObtenerUsuariosPorInteresesYHabilidadesQueryHandler(
    ILogger<ObtenerUsuariosPorInteresesYHabilidadesQueryHandler> logger,
    IRepositorioUsuario repositorioUsuario,
    IDistributedCache cache
    ) : IQueryHandler<
    ObtenerUsuariosPorInteresesYHabilidadesQuery, ResultadoPaginado<UsuarioReconmendacionDto>>
{
    public async Task<ResultadoT<ResultadoPaginado<UsuarioReconmendacionDto>>> Handle(
        ObtenerUsuariosPorInteresesYHabilidadesQuery request, CancellationToken cancellationToken)
    {
        if (request is { TamanoPagina: <= 0, NumeroPagina: <= 0 })
        {
            logger.LogWarning("Tamaño o número de página inválidos: TamañoPagina={Tamano}, NumeroPagina={Numero}",
                request.TamanoPagina, request.NumeroPagina);
            
            return ResultadoT<ResultadoPaginado<UsuarioReconmendacionDto>>.Fallo(
                Error.Fallo("400", "El número y tamaño de página deben ser mayores a cero."));
        }

        if (!request.HabilidadesIds.Any() || !request.InteresesIds.Any())
        {
            logger.LogWarning("Lista de intereses o habilidades vacía.");
        
            return ResultadoT<ResultadoPaginado<UsuarioReconmendacionDto>>.Fallo(
                Error.Fallo("400", "Debe especificar al menos un interés y una habilidad."));
        }

        var usuariosConInteresYHabilidades = await repositorioUsuario
            .FiltrarPorInteresesYHabilidadesAsync(request.InteresesIds, request.HabilidadesIds, cancellationToken);

        var usuariosList = usuariosConInteresYHabilidades as IList<Dominio.Modelos.Usuario> ?? usuariosConInteresYHabilidades.ToList();

        if (!usuariosList.Any())
        {
            logger.LogWarning("No se encontraron usuarios que coincidan con los intereses y habilidades proporcionados.");
            
            return ResultadoT<ResultadoPaginado<UsuarioReconmendacionDto>>.Fallo(
                Error.NoEncontrado("404", "No se encontraron usuarios con los intereses y habilidades especificados."));
        }
        
        var cacheLlave = $"usuarios-intereses-habilidades:{string.Join("-", request.InteresesIds)}:{string.Join("-", request.HabilidadesIds)}:{request.NumeroPagina}:{request.TamanoPagina}";
        
        var resultadoPaginado = await cache.ObtenerOCrearAsync(
            cacheLlave,
            async () =>
            {
                var usuariosDto = usuariosList
                    .Select(UsuarioMapper.MapToDto)
                    .ToList();

                var total = usuariosDto.Count;

                var paginados = usuariosDto
                    .Paginar(request.NumeroPagina, request.TamanoPagina)
                    .ToList();

                return new ResultadoPaginado<UsuarioReconmendacionDto>(paginados, total, request.NumeroPagina, request.TamanoPagina);
            },
            cancellationToken: cancellationToken
        );
        
        logger.LogInformation("Usuarios filtrados exitosamente por intereses y habilidades. Total encontrados: {TotalUsuarios}",
            resultadoPaginado.TotalElementos);

        return ResultadoT<ResultadoPaginado<UsuarioReconmendacionDto>>.Exito(resultadoPaginado);
    }

}