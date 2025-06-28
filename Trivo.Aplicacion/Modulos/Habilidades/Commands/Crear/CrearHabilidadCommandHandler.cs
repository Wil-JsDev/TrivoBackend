using Microsoft.Extensions.Logging;
using Trivo.Aplicacion.Abstracciones.Mensajes;
using Trivo.Aplicacion.DTOs.Habilidades;
using Trivo.Aplicacion.Interfaces.Repositorio;
using Trivo.Aplicacion.Interfaces.Repositorio.Cuenta;
using Trivo.Aplicacion.Utilidades;
using Trivo.Dominio.Modelos;

namespace Trivo.Aplicacion.Modulos.Habilidades.Commands.Crear;

internal sealed class CrearHabilidadCommandHandler(
    ILogger<CrearHabilidadCommandHandler> logger,
    IRepositorioHabilidad repositorioHabilidad,
    IRepositorioUsuario repositorioUsuario,
    IRepositorioUsuarioHabilidad repositorioUsuarioHabilidad
    ) :  ICommandHandler<CrearHabilidadCommand, HabilidadDto>
{
    public async Task<ResultadoT<HabilidadDto>> Handle(CrearHabilidadCommand request, CancellationToken cancellationToken)
    {

        if (request is null)
        {
            logger.LogWarning("Solicitud de creación de habilidades es nula.");

            return ResultadoT<HabilidadDto>.Fallo(Error.Fallo("400", "La solicitud enviada no puede ser nula."));
        }

        var usuario = await repositorioUsuario.ObtenerByIdAsync(request.UsearioId, cancellationToken);
        if (usuario is null)
        {
            logger.LogWarning("No se encontró el usuario con ID: {UsuarioId}", request.UsearioId);

            return ResultadoT<HabilidadDto>.Fallo(Error.NoEncontrado("404", "El usuario especificado no fue encontrado."));
        }

        var habilidad = new Habilidad()
        {
            HabilidadId = Guid.NewGuid(),
            Nombre = request.Nombre,
        };

        await repositorioHabilidad.CrearHabilidadAsync(habilidad, cancellationToken);

        logger.LogInformation("Se ha creado una nueva habilidad con ID {HabilidadId} para el usuario {UsuarioId}.",
            habilidad.HabilidadId, usuario.Id);

        var usuarioHabilidad = new UsuarioHabilidad()
        {
            UsuarioId = usuario.Id,
            HabilidadId = habilidad.HabilidadId,
            Nivel = request.Nivel.ToString()
        };

        await repositorioUsuarioHabilidad.CrearHabilidadesUsuarioAsync(usuarioHabilidad, cancellationToken);

        logger.LogInformation("Se ha asociado correctamente la habilidad {HabilidadId} al usuario {UsuarioId}.",
            habilidad.HabilidadId, usuario.Id);

        HabilidadDto habilidadDto = new 
        (
            HabilidadId: habilidad.HabilidadId,
            Nombre: habilidad.Nombre,
            FechaRegistro: habilidad.FechaRegistro
        );
        
        return ResultadoT<HabilidadDto>.Exito(habilidadDto);
    }
}