using Microsoft.Extensions.Logging;
using Trivo.Aplicacion.Abstracciones.Mensajes;
using Trivo.Aplicacion.DTOs.Email;
using Trivo.Aplicacion.Interfaces.Repositorio.Cuenta;
using Trivo.Aplicacion.Interfaces.Servicios;
using Trivo.Aplicacion.Utilidades;
using Trivo.Dominio.Enum;

namespace Trivo.Aplicacion.Modulos.Usuario.Commands.OlvidarContrsena;

internal sealed class OlvidarContrasenaUsuarioCommandHandler(
    IRepositorioUsuario repositorioUsuario,
    ILogger<OlvidarContrasenaUsuarioCommandHandler> logger,
    ICodigoServicio codigoServicio,
    IEmailServicio emailServicio
    ) : ICommandHandler<OlvidarContrasenaUsuarioCommand, string>
{
    public async Task<ResultadoT<string>> Handle(
        OlvidarContrasenaUsuarioCommand request, 
        CancellationToken cancellationToken)
    {
            var usuario = await repositorioUsuario.BuscarPorEmailUsuarioAsync(request.Email, cancellationToken);
            
            if (!await repositorioUsuario.ExisteEmailAsync(request.Email, cancellationToken))
            {
                logger.LogError("No se encontró ningún usuario con el correo electrónico '{Email}'.", request.Email);

                return ResultadoT<string>.Fallo(Error.NoEncontrado("404", "No existe ningún usuario con este correo"));
            }
            
            var codigo = await codigoServicio.GenerarCodigoAsync(usuario.Id ?? Guid.Empty, TipoCodigo.RecuperacionContrasena, cancellationToken);

            if (!codigo.EsExitoso)
            {
                logger.LogError("Falló la generación del código para el usuario '{UsuarioId}'. Error: {Error}",
                    usuario.Id, codigo.Error!.Descripcion);
        
                return ResultadoT<string>.Fallo(codigo.Error!);
            } 
            
            await emailServicio.EnviarEmailAsync(
                new EmailRespuestaDto(
                    Usuario: usuario.Email!,
                    Cuerpo: EmailTemas.RecuperacionDeContrasena(usuario.NombreUsuario!, codigo.Valor!),
                    Tema: "Olvidaste la contraseña"
                )
            );

            logger.LogInformation("Se envió el código de recuperación al correo '{Email}' para el usuario '{UsuarioId}'.",
                usuario.Email, usuario.Id);

            return ResultadoT<string>.Exito("Se envió el código de recuperación correctamente.");
    }
}