using Microsoft.Extensions.Logging;
using Trivo.Aplicacion.Abstracciones.Mensajes;
using Trivo.Aplicacion.DTOs.Cuentas.Expertos;
using Trivo.Aplicacion.Interfaces.Repositorio;
using Trivo.Aplicacion.Interfaces.Repositorio.Cuenta;
using Trivo.Aplicacion.Modulos.Usuario.Commands.Crear;
using Trivo.Aplicacion.Utilidades;

namespace Trivo.Aplicacion.Modulos.Experto.Commands.Actualizar;

public sealed class ActualizarExpertoCommandHandler(
    IRepositorioExperto repositorioExperto,
    ILogger<ActualizarExpertoCommandHandler> logger
    ): ICommandHandler<ActualizarExpertoCommand, ExpertoDto>
{
    public async Task<ResultadoT<ExpertoDto>> Handle(ActualizarExpertoCommand request, CancellationToken cancellationToken)
    {
        if (request is null)
        {
            logger.LogWarning("Se recibio un ActualizarExpertoCommand nulo.");
            return ResultadoT<ExpertoDto>.Fallo(Error.Fallo("400", "La solicitud no puede ser nula."));
        }

        logger.LogInformation("Iniciando actualización del experto con Id {ExpertoId}.", request.ExpertoId);

        var experto = await repositorioExperto.ObtenerByIdAsync(request.ExpertoId, cancellationToken);

        if (experto is null)
        {
            logger.LogWarning("No se encontro el experto con Id {ExpertoId}.", request.ExpertoId);
            return ResultadoT<ExpertoDto>.Fallo(Error.NoEncontrado("404", "El experto no se pudo encontrar"));
        }

        experto.DisponibleParaProyectos = request.DisponibleParaProyectos;
        experto.Contratado = request.Contratado;

        await repositorioExperto.ActualizarAsync(experto, cancellationToken);

        logger.LogInformation("Experto con Id {ExpertoId} actualizado correctamente.", experto.Id);

        var dto = new ExpertoDto(
            experto.Id!.Value,
            experto.UsuarioId!.Value,
            experto.DisponibleParaProyectos ?? false,
            experto.Contratado ?? false
        );

        return ResultadoT<ExpertoDto>.Exito(dto);
    }
}
