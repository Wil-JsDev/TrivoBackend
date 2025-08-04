using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using Trivo.Aplicacion.Abstracciones.Mensajes;
using Trivo.Aplicacion.DTOs.Cuentas.Usuarios;
using Trivo.Aplicacion.DTOs.Emparejamiento;
using Trivo.Aplicacion.Helper;
using Trivo.Aplicacion.Interfaces.Repositorio;
using Trivo.Aplicacion.Interfaces.Repositorio.Cuenta;
using Trivo.Aplicacion.Interfaces.Servicios.SignaIR;
using Trivo.Aplicacion.Mapper;
using Trivo.Aplicacion.Utilidades;
using Trivo.Dominio.Enum;

namespace Trivo.Aplicacion.Modulos.Emparejamiento.Querys;

internal sealed class ObtenerEmparejamientoPorUsuarioQueryHandler(
    ILogger<ObtenerEmparejamientoPorUsuarioQueryHandler> logger,
    IRepositorioEmparejamiento repositorioEmparejamiento,
    IRepositorioUsuario repositorioUsuario,
    INotificadorDeEmparejamiento emparejamientoNotificador,
    IRepositorioExperto repositorioExperto,
    IRepositorioReclutador repositorioReclutador,
    IDistributedCache cache
    ) : IQueryHandler<ObtenerEmparejamientoPorUsuarioQuery, IEnumerable<EmparejamientoDto>>
{
    public async Task<ResultadoT<IEnumerable<EmparejamientoDto>>> Handle(ObtenerEmparejamientoPorUsuarioQuery request, CancellationToken cancellationToken)
    {
        if (request is null)
        {
            logger.LogWarning("La solicitud de obtención de emparejamientos es nula.");
            
            return ResultadoT<IEnumerable<EmparejamientoDto>>.Fallo(Error.Fallo("400", "La solicitud no puede ser nula."));
        }

        var usuario = await repositorioUsuario.ObtenerByIdAsync(request.UsuarioId, cancellationToken);
        if (usuario is null)
        {
            logger.LogWarning("No se encontró el usuario con ID {UsuarioId}.", request.UsuarioId);
            
            return ResultadoT<IEnumerable<EmparejamientoDto>>.Fallo(Error.NoEncontrado("404", "El usuario no existe"));
        }
        var reclutador = await repositorioReclutador.ObtenerReclutadorPorUsuarioIdAsync(request.UsuarioId, cancellationToken);

        var experto = await repositorioExperto.ObtenerExpertoPorUsuarioIdAsync(request.UsuarioId, cancellationToken);
        
        var estrategiasEmparejamiento = Obtener(request.UsuarioId);

        if (!estrategiasEmparejamiento.TryGetValue(request.Rol, out var filtroEmparejamiento))
        {
            logger.LogWarning("El rol proporcionado '{Rol}' no tiene una estrategia de emparejamiento válida.", request.Rol);
            
            return ResultadoT<IEnumerable<EmparejamientoDto>>.Fallo(Error.Fallo("400", "El rol proporcionado no es válido para emparejamientos."));
        }

        var emparejamientos = await filtroEmparejamiento(cancellationToken);

        IEnumerable<Dominio.Modelos.Emparejamiento> enumerable = emparejamientos.ToList();
        var emparejamientosLista = enumerable.ToList();

        // var emparejamientoDto = emparejamientosLista
        //     .Select(e => e.EmparejamientoDto(request.Rol))
        //     .ToList();

        // List<EmparejamientoDto> emparejamientoDto;
        //
        // if (request.Rol == Roles.Experto)
        // {
        //     emparejamientoDto = emparejamientosLista.Select(e =>
        //     {
        //         var expertoDto = EmparejamientoMapper.MappearExpertoReconmendacionDto(
        //             e.Experto!.Usuario!,
        //             e.Experto
        //         );
        //
        //         return new EmparejamientoDto(
        //             EmparejamientoId: e.Id ?? Guid.Empty,
        //             ReclutadotId: null,
        //             ExpertoId: e.Experto!.Id ?? Guid.Empty,
        //             ExpertoEstado: e.ExpertoEstado ?? string.Empty,
        //             ReclutadorEstado: e.ReclutadorEstado ?? string.Empty,
        //             EmparejamientoEstado: e.EmparejamientoEstado ?? string.Empty,
        //             FechaRegistro: e.FechaRegistro,
        //             UsuarioReconmendacionDto: expertoDto
        //         );
        //     }).ToList();
        // }
        // else if (request.Rol == Roles.Reclutador)
        // {
        //     emparejamientoDto = emparejamientosLista.Select(e =>
        //     {
        //         var reclutadorDto = EmparejamientoMapper.MappearReclutadorReconmendacionDto(
        //             e.Reclutador!.Usuario!,
        //             e.Reclutador
        //         );
        //
        //         return new EmparejamientoDto(
        //             EmparejamientoId: e.Id ?? Guid.Empty,
        //             ReclutadotId: e.Reclutador!.Id ?? Guid.Empty,
        //             ExpertoId: null,
        //             ExpertoEstado: e.ExpertoEstado ?? string.Empty,
        //             ReclutadorEstado: e.ReclutadorEstado ?? string.Empty,
        //             EmparejamientoEstado: e.EmparejamientoEstado ?? string.Empty,
        //             FechaRegistro: e.FechaRegistro,
        //             UsuarioReconmendacionDto: reclutadorDto
        //         );
        //     }).ToList();
        // }
        List<EmparejamientoDto> emparejamientoDto;

        if (request.Rol == Roles.Experto)
        {
            // Yo soy experto, entonces el DTO debe traer datos del reclutador
            emparejamientoDto = emparejamientosLista.Select(e =>
            {
                var reclutadorDto = EmparejamientoMapper.MappearReclutadorReconmendacionDto(
                    e.Reclutador!.Usuario!,
                    e.Reclutador
                );

                return new EmparejamientoDto(
                    EmparejamientoId: e.Id ?? Guid.Empty,
                    ReclutadotId: e.Reclutador!.Id ?? Guid.Empty,
                    ExpertoId: e.Experto!.Id ?? Guid.Empty,
                    ExpertoEstado: e.ExpertoEstado ?? string.Empty,
                    ReclutadorEstado: e.ReclutadorEstado ?? string.Empty,
                    EmparejamientoEstado: e.EmparejamientoEstado ?? string.Empty,
                    FechaRegistro: e.FechaRegistro,
                    UsuarioReconmendacionDto: reclutadorDto // Datos del otro usuario
                );
            }).ToList();
        }
        else if (request.Rol == Roles.Reclutador)
        {
            // Yo soy reclutador, entonces el DTO debe traer datos del experto
            emparejamientoDto = emparejamientosLista.Select(e =>
            {
                var expertoDto = EmparejamientoMapper.MappearExpertoReconmendacionDto(
                    e.Experto!.Usuario!,
                    e.Experto
                );

                return new EmparejamientoDto(
                    EmparejamientoId: e.Id ?? Guid.Empty,
                    ReclutadotId: e.Reclutador!.Id ?? Guid.Empty,
                    ExpertoId: e.Experto!.Id ?? Guid.Empty,
                    ExpertoEstado: e.ExpertoEstado ?? string.Empty,
                    ReclutadorEstado: e.ReclutadorEstado ?? string.Empty,
                    EmparejamientoEstado: e.EmparejamientoEstado ?? string.Empty,
                    FechaRegistro: e.FechaRegistro,
                    UsuarioReconmendacionDto: expertoDto // Datos del otro usuario
                );
            }).ToList();
        }
        else
        {
            // Opcional: manejar otros roles o error
            emparejamientoDto = new List<EmparejamientoDto>();
        }
        
        var emparejamientoPaginadoDto = emparejamientoDto
            .Paginar(request.NumeroPagina, request.TamanoPagina)
            .ToList();
        
        logger.LogInformation("Se recuperaron correctamente {Cantidad} emparejamientos para el usuario {UsuarioId} con rol {Rol}.",
            emparejamientosLista.Count, request.UsuarioId, request.Rol);
        
        if (!emparejamientoDto.Any())
        {
            logger.LogWarning("No se encontraron emparejamientos pendientes para el usuario {UsuarioId} con el rol {Rol}.", request.UsuarioId, request.Rol);
            
            return ResultadoT<IEnumerable<EmparejamientoDto>>.Fallo(Error.Fallo("404", "No se encontraron emparejamientos para el usuario."));
        }
        
        logger.LogInformation("Se recuperaron correctamente {Cantidad} emparejamientos para el usuario {UsuarioId} con rol {Rol}.",
            emparejamientosLista.Count, request.UsuarioId, request.Rol);

        if (reclutador is null || experto is null)
        {
            logger.LogInformation("La notificación fue enviada solo a uno de los usuarios debido a que el otro no aplica para este contexto (reclutador o experto es null).");
        }
        else
        {
            await emparejamientoNotificador.NotificarEmparejamiento(
                reclutador.Id!.Value,
                experto.Id!.Value,
                emparejamientoPaginadoDto,
                emparejamientoPaginadoDto
            );
            logger.LogInformation("Se notificó exitosamente a ambos usuarios (reclutador y experto) por SignalR.");
        }
        
        return ResultadoT<IEnumerable<EmparejamientoDto>>.Exito(emparejamientoPaginadoDto);
    }
    
    #region Privados

        private Dictionary<Roles, Func<CancellationToken, Task<IEnumerable<Dominio.Modelos.Emparejamiento>>>> Obtener(Guid usuarioId)
        {
            return new Dictionary<Roles, Func<CancellationToken, Task<IEnumerable<Dominio.Modelos.Emparejamiento>>>>
            {
                { Roles.Experto, async cancellationToken => await repositorioEmparejamiento.ObtenerEmparejamientosComoExpertoAsync(usuarioId,cancellationToken) },
                { Roles.Reclutador, async cancellationToken => await repositorioEmparejamiento.ObtenerEmparejamientosComoReclutadorAsync(usuarioId,cancellationToken)}
            };
        }

    #endregion
}