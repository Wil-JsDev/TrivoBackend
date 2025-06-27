using Microsoft.Extensions.Logging;
using Trivo.Aplicacion.Abstracciones.Mensajes;
using Trivo.Aplicacion.DTOs.Cuentas.Expertos;
using Trivo.Aplicacion.Interfaces.Repositorio;
using Trivo.Aplicacion.Interfaces.Repositorio.Cuenta;
using Trivo.Aplicacion.Utilidades;

namespace Trivo.Aplicacion.Modulos.Experto.Commands.Crear;

internal sealed class CrearExpertoCommandHandler(
    IRepositorioExperto repositorioExperto,
    IRepositorioUsuario repositorioUsuario,
    ILogger<CrearExpertoCommandHandler> logger
    ) : ICommandHandler<CrearExpertoCommand, ExpertoDto>
{
    public async Task<ResultadoT<ExpertoDto>> Handle(CrearExpertoCommand request, CancellationToken cancellationToken)
    {
        if (request is not null)
        {
            var usuario = await repositorioUsuario.ObtenerByIdAsync(request.UsuarioId, cancellationToken);
            if (usuario is null)
            {
                logger.LogError($"El usuario con id {request.UsuarioId} no existe");
                return ResultadoT<ExpertoDto>.Fallo(Error.NoEncontrado("404", "El usuario no existe"));

            }   
            
            var experto = new Dominio.Modelos.Experto
            {
                Id = Guid.NewGuid(),
                UsuarioId = request.UsuarioId,
                DisponibleParaProyectos = request.DisponibleParaProyectos,
                Contratado = request.Contratado
            };
            
            await repositorioExperto.CrearAsync(experto, cancellationToken);
            logger.LogInformation("Experto '{ExpertoId}' creado correctamente.", experto.Id);

            var dto = new ExpertoDto(
                experto.Id.Value,
                experto.UsuarioId!.Value,
                experto.DisponibleParaProyectos ?? false,
                experto.Contratado ?? false
            );  
            
            return ResultadoT<ExpertoDto>.Exito(dto);
        }
       
        logger.LogWarning("Se recibio un CrearExpertoCommand nulo.");
        return ResultadoT<ExpertoDto>.Fallo(Error.Fallo("400", "La solicitud no puede ser nula."));
    }
}