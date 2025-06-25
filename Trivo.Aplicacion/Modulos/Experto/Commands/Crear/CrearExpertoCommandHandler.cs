using Trivo.Aplicacion.Abstracciones.Mensajes;
using Trivo.Aplicacion.DTOs.Cuentas.Expertos;
using Trivo.Aplicacion.Interfaces.Repositorio;
using Trivo.Aplicacion.Interfaces.Servicios;
using Trivo.Aplicacion.Utilidades;
using Microsoft.Extensions.Logging;
using Trivo.Aplicacion.Interfaces.Repositorio.Cuenta;
using Trivo.Aplicacion.Modulos.Usuario.Commands.Crear;


namespace Trivo.Aplicacion.Modulos.Experto.Commands;

internal sealed class CrearExpertoCommandHandler(
    IRepositorioExperto repositorioExperto,
    IRepositorioUsuario repositorioUsuario,
    ILogger<CrearExpertoCommandHandler> logger
    ) : ICommandHandler<CrearExpertoCommand, ExpertoDto>
{
    public async Task<ResultadoT<ExpertoDto>> Handle(CrearExpertoCommand solicitud, CancellationToken cancellationToken)
    {
        if (solicitud is not null)
        {
            var usuario = await repositorioUsuario.ObtenerByIdAsync(solicitud.UsuarioId, cancellationToken);
            if (usuario is null)
            {
                logger.LogError($"El usuario con id {solicitud.UsuarioId} no existe");
                return ResultadoT<ExpertoDto>.Fallo(Error.NoEncontrado("404", "El usuario no existe"));

            }   
            
            var experto = new Dominio.Modelos.Experto
            {
                Id = Guid.NewGuid(),
                UsuarioId = solicitud.UsuarioId,
                DisponibleParaProyectos = solicitud.DisponibleParaProyectos,
                Contratado = solicitud.Contratado
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