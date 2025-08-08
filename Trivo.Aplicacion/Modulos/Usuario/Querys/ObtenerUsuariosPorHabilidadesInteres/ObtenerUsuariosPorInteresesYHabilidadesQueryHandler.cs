using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using Trivo.Aplicacion.Abstracciones.Mensajes;
using Trivo.Aplicacion.DTOs.Cuentas.Expertos;
using Trivo.Aplicacion.DTOs.Cuentas.Usuarios;
using Trivo.Aplicacion.DTOs.Habilidades;
using Trivo.Aplicacion.DTOs.Intereses;
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
    ObtenerUsuariosPorInteresesYHabilidadesQuery, ResultadoPaginado<UsuarioReconmendacionInteresHabilidadDto>>
{
    public async Task<ResultadoT<ResultadoPaginado<UsuarioReconmendacionInteresHabilidadDto>>> Handle(
        ObtenerUsuariosPorInteresesYHabilidadesQuery request, CancellationToken cancellationToken)
    {
        if (request is { TamanoPagina: <= 0, NumeroPagina: <= 0 })
        {
            logger.LogWarning("Tamaño o número de página inválidos: TamañoPagina={Tamano}, NumeroPagina={Numero}",
                request.TamanoPagina, request.NumeroPagina);
            
            return ResultadoT<ResultadoPaginado<UsuarioReconmendacionInteresHabilidadDto>>.Fallo(
                Error.Fallo("400", "El número y tamaño de página deben ser mayores a cero."));
        }
        
        var usuariosConInteresYHabilidades = await repositorioUsuario
            .FiltrarPorInteresesYHabilidadesAsync(request.InteresesIds, request.HabilidadesIds, cancellationToken);

        var usuariosList = usuariosConInteresYHabilidades as IList<Dominio.Modelos.Usuario> ?? usuariosConInteresYHabilidades.ToList();

        if (!usuariosList.Any())
        {
            logger.LogWarning("No se encontraron usuarios que coincidan con los intereses y habilidades proporcionados.");
            
            return ResultadoT<ResultadoPaginado<UsuarioReconmendacionInteresHabilidadDto>>.Fallo(
                Error.NoEncontrado("404", "No se encontraron usuarios con los intereses y habilidades especificados."));
        }
        
        var cacheLlave = $"usuarios-intereses-habilidades:{string.Join("-", request.InteresesIds)}:{string.Join("-", request.HabilidadesIds)}:{request.NumeroPagina}:{request.TamanoPagina}";
        
        // var resultadoPaginado = await cache.ObtenerOCrearAsync(
        //     cacheLlave,
        //     async () =>
        //     {
        //         
        //         List<UsuarioReconmendacionInteresHabilidadDto> usuariosDto = new();
        //
        //         if (usuariosList.FirstOrDefault()!.Reclutadores!.Any())
        //         {
        //             usuariosDto = usuariosList.Select(x => new UsuarioReconmendacionInteresHabilidadDto
        //             (
        //                 UsuarioId: x.Id ?? Guid.Empty,
        //                 Nombre: x.Nombre,
        //                 Apellido: x.Apellido,
        //                 Biografia: x.Biografia,
        //                 Posicion: x.Posicion,
        //                 Intereses: UsuarioMapper.MappearAintereses(x.UsuarioInteres),
        //                 Habilidades: UsuarioMapper.MappearAHabilidades(x.UsuarioHabilidades),
        //                 FotoPerfil: x.FotoPerfil,
        //                 ExpertoDto: RecomendacionMapper.MappearAExpertoDto(x.Expertos!.FirstOrDefault()!.Usuario!, x.Expertos.FirstOrDefault()),
        //                 ReclutadorDto: null
        //             )).ToList();
        //         }
        //         
        //         if (usuariosList.FirstOrDefault()!.Expertos!.Any())
        //         {
        //             usuariosDto = usuariosList.Select(x => new UsuarioReconmendacionInteresHabilidadDto
        //             (
        //                 UsuarioId: x.Id ?? Guid.Empty,
        //                 Nombre: x.Nombre,
        //                 Apellido: x.Apellido,
        //                 Biografia: x.Biografia,
        //                 Posicion: x.Posicion,
        //                 Intereses: UsuarioMapper.MappearAintereses(x.UsuarioInteres),
        //                 Habilidades: UsuarioMapper.MappearAHabilidades(x.UsuarioHabilidades),
        //                 FotoPerfil: x.FotoPerfil,
        //                 ExpertoDto: null,
        //                 ReclutadorDto:  RecomendacionMapper.MappearAReclutadorDto(x.Reclutadores!.FirstOrDefault()!.Usuario!, x.Reclutadores.FirstOrDefault())
        //             )).ToList();
        //         }
        //         
        //         var total = usuariosDto.Count;
        //
        //         var paginados = usuariosDto
        //             .Paginar(request.NumeroPagina, request.TamanoPagina)
        //             .ToList();
        //
        //         return new ResultadoPaginado<UsuarioReconmendacionInteresHabilidadDto>(paginados, total, request.NumeroPagina, request.TamanoPagina);
        //     },
        //     cancellationToken: cancellationToken
        // );
        
         var resultadoPaginado = await cache.ObtenerOCrearAsync(
        cacheLlave,
        async () =>
        {
            var usuariosDto = usuariosList.Select(u =>
            {
                var intereses = UsuarioMapper.MappearAintereses(u.UsuarioInteres);
                var habilidades = UsuarioMapper.MappearAHabilidades(u.UsuarioHabilidades);

                if (u.Reclutadores is not null && u.Reclutadores.Any())
                {
                    var reclutador = u.Reclutadores.First();
                    return new UsuarioReconmendacionInteresHabilidadDto(
                        // UsuarioId: u.Id ?? Guid.Empty,
                        // Nombre: u.Nombre,
                        // Apellido: u.Apellido,
                        // Biografia: u.Biografia,
                        // Posicion: u.Posicion,
                        // Intereses: intereses,
                        // Habilidades: habilidades,
                        // FotoPerfil: u.FotoPerfil,
                        ExpertoDto: null,
                        ReclutadorDto: RecomendacionMapper.MappearAReclutadorDto(reclutador.Usuario!, reclutador)
                    );
                }
                else if (u.Expertos is not null && u.Expertos.Any())
                {
                    var experto = u.Expertos.First();
                    return new UsuarioReconmendacionInteresHabilidadDto(
                        // UsuarioId: u.Id ?? Guid.Empty,
                        // Nombre: u.Nombre,
                        // Apellido: u.Apellido,
                        // Biografia: u.Biografia,
                        // Posicion: u.Posicion,
                        // Intereses: intereses,
                        // Habilidades: habilidades,
                        // FotoPerfil: u.FotoPerfil,
                        ExpertoDto: RecomendacionMapper.MappearAExpertoDto(experto.Usuario!, experto),
                        ReclutadorDto: null
                    );
                }
                else
                {
                    return new UsuarioReconmendacionInteresHabilidadDto(
                        // UsuarioId: u.Id ?? Guid.Empty,
                        // Nombre: u.Nombre,
                        // Apellido: u.Apellido,
                        // Biografia: u.Biografia,
                        // Posicion: u.Posicion,
                        // Intereses: intereses,
                        // Habilidades: habilidades,
                        // FotoPerfil: u.FotoPerfil,
                        ExpertoDto: null,
                        ReclutadorDto: null
                    );
                }
            }).ToList();

            var total = usuariosDto.Count;

            var paginados = usuariosDto
                .Paginar(request.NumeroPagina, request.TamanoPagina)
                .ToList();

            return new ResultadoPaginado<UsuarioReconmendacionInteresHabilidadDto>(paginados, total, request.NumeroPagina, request.TamanoPagina);
        },
        cancellationToken: cancellationToken
    );
    
        
        logger.LogInformation("Usuarios filtrados exitosamente por intereses y habilidades. Total encontrados: {TotalUsuarios}",
            resultadoPaginado.TotalElementos);

        return ResultadoT<ResultadoPaginado<UsuarioReconmendacionInteresHabilidadDto>>.Exito(resultadoPaginado);
    }

}