using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using Trivo.Aplicacion.Abstracciones.Mensajes;
using Trivo.Aplicacion.DTOs.Cuentas.Expertos;
using Trivo.Aplicacion.DTOs.Cuentas.Reclutador;
using Trivo.Aplicacion.DTOs.Cuentas.Usuarios;
using Trivo.Aplicacion.Interfaces.Repositorio.Cuenta;
using Trivo.Aplicacion.Mapper;
using Trivo.Aplicacion.Utilidades;

namespace Trivo.Aplicacion.Modulos.Usuario.Querys.ObtenerDetalles;

internal sealed class ObtenerDetallesUsuarioQueryHandler(
    IRepositorioUsuario repositorioUsuario,
    IRepositorioExperto repositorioExperto,
    IRepositorioReclutador repositorioReclutador,
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
        
        if (await repositorioExperto.EsUsuarioExpertoAsync(request.UsuarioId, cancellationToken))
        {
            var dto = await cache.ObtenerOCrearAsync(
                $"obtener-detalles-experto-usuario-id-{request.UsuarioId}",
                async () =>
                {
                    var experto = await repositorioExperto.ObtenerDetallesExpertoAsync(request.UsuarioId, cancellationToken);

                    return new UsuarioDetallesExpertoDto(
                        Nombre: usuarioDetalles.Nombre,
                        Apellido: usuarioDetalles.Apellido,
                        Ubicacion: usuarioDetalles.Ubicacion,
                        Biografia: usuarioDetalles.Biografia,
                        FotoPerfil: usuarioDetalles.FotoPerfil,
                        Posicion: usuarioDetalles.Posicion,
                        Habilidad: HabilidadesMapper.MapHabilidades(usuarioDetalles.UsuarioHabilidades),
                        Interes: MapperInteres.MapIntereses(usuarioDetalles.UsuarioInteres),
                        DisponibleParaProyectos: experto?.DisponibleParaProyectos,
                        Contratado: experto?.Contratado
                    );
                },
                cancellationToken: cancellationToken
            );

            logger.LogInformation("Se obtuvieron correctamente los detalles del experto.");

            return ResultadoT<UsuarioDetallesDto>.Exito(dto);

        }
        
        if (await repositorioReclutador.EsUsuarioReclutadorAsync(request.UsuarioId, cancellationToken))
        {
            var dto = await cache.ObtenerOCrearAsync(
                $"obtener-detalles-reclutador-usuario-id-{request.UsuarioId}",
                async () =>
                {
                    var reclutador = await repositorioReclutador.ObtenerDetallesReclutadorAsync(request.UsuarioId, cancellationToken);

                    return new UsuarioDetallesReclutadorDto(
                        Nombre: usuarioDetalles.Nombre,
                        Apellido: usuarioDetalles.Apellido,
                        Ubicacion: usuarioDetalles.Ubicacion,
                        Biografia: usuarioDetalles.Biografia,
                        FotoPerfil: usuarioDetalles.FotoPerfil,
                        Posicion: usuarioDetalles.Posicion,
                        Habilidad: HabilidadesMapper.MapHabilidades(usuarioDetalles.UsuarioHabilidades),
                        Interes: MapperInteres.MapIntereses(usuarioDetalles.UsuarioInteres),
                        NombreEmpresa: reclutador?.NombreEmpresa
                    );
                },
                cancellationToken: cancellationToken
            );

            logger.LogInformation("Se obtuvieron correctamente los detalles del reclutador.");

            return ResultadoT<UsuarioDetallesDto>.Exito(dto);

        }
        
        UsuarioDetallesDto usuarioDetallesDto = new
        (
            Nombre: usuarioDetalles.Nombre,
            Apellido: usuarioDetalles.Apellido,
            Ubicacion: usuarioDetalles.Ubicacion,
            Biografia: usuarioDetalles.Biografia,
            FotoPerfil: usuarioDetalles.FotoPerfil,
            Posicion: usuarioDetalles.Posicion,
            Habilidad: HabilidadesMapper.MapHabilidades(usuarioDetalles.UsuarioHabilidades),
            Interes: MapperInteres.MapIntereses(usuarioDetalles.UsuarioInteres)
        );

        logger.LogInformation("Se obtuvieron correctamente los detalles del usuario con ID '{UsuarioId}'.", usuarioDetalles.Id);

        return ResultadoT<UsuarioDetallesDto>.Exito(usuarioDetallesDto);

    }
}