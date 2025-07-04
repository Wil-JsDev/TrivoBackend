using Microsoft.Extensions.Logging;
using Trivo.Aplicacion.Abstracciones.Mensajes;
using Trivo.Aplicacion.DTOs.Cuentas.Reclutador;
using Trivo.Aplicacion.Interfaces.Repositorio;
using Trivo.Aplicacion.Interfaces.Repositorio.Cuenta;
using Trivo.Aplicacion.Utilidades;

namespace Trivo.Aplicacion.Modulos.Reclutador.Commands.Crear;

internal sealed class CrearReclutadorCommandHandler(
    IRepositorioReclutador repositorioReclutador, 
    IRepositorioUsuario repositorioUsuario,
    ILogger<CrearReclutadorCommandHandler> logger
) : ICommandHandler<CrearReclutadorCommand, ReclutadorDto>
{
    public async Task<ResultadoT<ReclutadorDto>> Handle(CrearReclutadorCommand request, CancellationToken cancellationToken)
    {
        if (request is null)
        {
            logger.LogWarning("Se recibió un CrearReclutadorCommand nulo.");
            return ResultadoT<ReclutadorDto>.Fallo(Error.Fallo("400", "La solicitud no puede ser nula."));
        }

        var usuario = await repositorioUsuario.ObtenerByIdAsync(request.UsuarioId, cancellationToken);
        if (usuario is null)
        {
            logger.LogError("El usuario con id {UsuarioId} no existe.", request.UsuarioId);
            
            return ResultadoT<ReclutadorDto>.Fallo(Error.NoEncontrado("404", "El usuario no existe"));
        }
        
        if (usuario.CuentaConfirmada is false)
        {
            logger.LogWarning("Intento de crear un experto con una cuenta no confirmada. UsuarioId: {UsuarioId}", usuario.Id);
                
            return ResultadoT<ReclutadorDto>.Fallo(Error.Fallo("403", "El usuario debe confirmar su cuenta para poder crear un experto"));
        }

        var reclutador = new Dominio.Modelos.Reclutador
        {
            Id = Guid.NewGuid(),
            NombreEmpresa = request.NombreEmpresa,
            UsuarioId = request.UsuarioId,
        };

        await repositorioReclutador.CrearAsync(reclutador, cancellationToken);
        logger.LogInformation("Reclutador '{ReclutadorId}' creado correctamente para el usuario '{UsuarioId}'.", reclutador.Id, request.UsuarioId);

        var dto = new ReclutadorDto(
            Id: reclutador.Id!.Value,
            NombreEmpresa: request.NombreEmpresa,
            UsuarioId: request.UsuarioId
        );

        return ResultadoT<ReclutadorDto>.Exito(dto);
    }
}