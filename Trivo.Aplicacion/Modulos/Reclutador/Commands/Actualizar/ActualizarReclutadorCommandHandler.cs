using Microsoft.Extensions.Logging;
using Trivo.Aplicacion.Abstracciones.Mensajes;
using Trivo.Aplicacion.DTOs.Cuentas.Reclutador;
using Trivo.Aplicacion.Interfaces.Repositorio.Cuenta;
using Trivo.Aplicacion.Utilidades;

namespace Trivo.Aplicacion.Modulos.Reclutador.Commands.Actualizar;

internal sealed class ActualizarReclutadorCommandHandler(
    IRepositorioReclutador repositorioReclutador,
    ILogger<ActualizarReclutadorCommandHandler> logger
) : ICommandHandler<ActualizarReclutadorCommand, ReclutadorDto>
{
    public async Task<ResultadoT<ReclutadorDto>> Handle(ActualizarReclutadorCommand request, CancellationToken cancellationToken)
    {
        if (request is null)
        {
            logger.LogWarning("Se recibió una solicitud nula para actualizar un reclutador.");
            return ResultadoT<ReclutadorDto>.Fallo(Error.Fallo("400", "La solicitud no puede ser nula."));
        }

        var reclutador = await repositorioReclutador.ObtenerByIdAsync(request.ReclutadorId, cancellationToken);
        if (reclutador is null)
        {
            logger.LogError("No se encontró el reclutador con el ID {ReclutadorId}.", request.ReclutadorId);
            return ResultadoT<ReclutadorDto>.Fallo(Error.NoEncontrado("404", "El reclutador no existe."));
        }

        reclutador.NombreEmpresa = request.NombreEmpresa;

        await repositorioReclutador.ActualizarAsync(reclutador, cancellationToken);
        logger.LogInformation("Reclutador '{ReclutadorId}' actualizado correctamente.", reclutador.Id);

        var dto = new ReclutadorDto(
            reclutador.Id!.Value,
            reclutador.NombreEmpresa!,
            reclutador.UsuarioId!.Value
        );

        return ResultadoT<ReclutadorDto>.Exito(dto);
    }
}