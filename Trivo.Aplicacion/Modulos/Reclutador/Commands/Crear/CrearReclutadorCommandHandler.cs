using Microsoft.Extensions.Logging;
using Trivo.Aplicacion.Abstracciones.Mensajes;
using Trivo.Aplicacion.DTOs.Cuentas.Reclutador;
using Trivo.Aplicacion.Interfaces.Repositorio;
using Trivo.Aplicacion.Interfaces.Repositorio.Cuenta;
using Trivo.Aplicacion.Utilidades;

namespace Trivo.Aplicacion.Modulos.Reclutador.Commands.Actualizar;

internal sealed class CrearReclutadorCommandHandler(
    IRepositorioReclutador repositorioReclutador, 
    IRepositorioUsuario repositorioUsuario,
    ILogger<CrearReclutadorCommandHandler> logger
) : ICommandHandler<CrearReclutadorCommand, ReclutadorDto>
{
    public async Task<ResultadoT<ReclutadorDto>> Handle(CrearReclutadorCommand solicitud, CancellationToken cancellationToken)
    {
        if (solicitud == null)
        {
            logger.LogWarning("Se recibió un CrearReclutadorCommand nulo.");
            return ResultadoT<ReclutadorDto>.Fallo(Error.Fallo("400", "La solicitud no puede ser nula."));
        }

        var usuario = await repositorioUsuario.ObtenerByIdAsync(solicitud.UsuarioId, cancellationToken);
        if (usuario is null)
        {
            logger.LogError("El usuario con id {UsuarioId} no existe.", solicitud.UsuarioId);
            return ResultadoT<ReclutadorDto>.Fallo(Error.NoEncontrado("404", "El usuario no existe"));
        }

        var reclutador = new Dominio.Modelos.Reclutador
        {
            Id = Guid.NewGuid(),
            NombreEmpresa = solicitud.NombreEmpresa,
            UsuarioId = solicitud.UsuarioId,
        };

        await repositorioReclutador.CrearAsync(reclutador, cancellationToken);
        logger.LogInformation("Reclutador '{ReclutadorId}' creado correctamente para el usuario '{UsuarioId}'.", reclutador.Id, solicitud.UsuarioId);

        var dto = new ReclutadorDto(
            Id: reclutador.Id!.Value,
            NombreEmpresa: solicitud.NombreEmpresa,
            UsuarioId: solicitud.UsuarioId
        );

        return ResultadoT<ReclutadorDto>.Exito(dto);
    }
}