using Microsoft.Extensions.Logging;
using Trivo.Aplicacion.Abstracciones.Mensajes;
using Trivo.Aplicacion.Interfaces.Repositorio.Cuenta;
using Trivo.Aplicacion.Utilidades;

namespace Trivo.Aplicacion.Modulos.Usuario.Commands.ModificarContrasena;

internal sealed class ModificarContrasenaUsuarioCommandHandler(
    ILogger<ModificarContrasenaUsuarioCommandHandler> logger,
    IRepositorioUsuario repositorioUsuario,
    IRepositorioCodigo repositorioCodigo
    ) : ICommandHandler<ModificarContrasenaUsuarioCommand, string>
{
    public async Task<ResultadoT<string>> Handle(
        ModificarContrasenaUsuarioCommand request, 
        CancellationToken cancellationToken
    )
    {
        if (request == null)
        {
            logger.LogWarning("La solicitud para actualizar la contraseña es nula.");
            
            return ResultadoT<string>.Fallo(Error.Fallo("400", "La solicitud no puede ser nula."));
        }

        var usuario = await repositorioUsuario.ObtenerByIdAsync(request.UsuarioId, cancellationToken);
        if (usuario == null)
        {
            logger.LogWarning("No se encontró el usuario con ID '{RequestUsuarioId}'.", request.UsuarioId);
            
            return ResultadoT<string>.Fallo(Error.NoEncontrado("404", "El usuario no fue encontrado."));
        }
        
        var codigo = await repositorioCodigo.BuscarCodigoAsync(request.Codigo, cancellationToken);
        if (codigo is null)
        {
            logger.LogWarning("El código ingresado no fue encontrado: {Codigo}", request.Codigo);
    
            return ResultadoT<string>.Fallo(Error.NoEncontrado("404", "El código ingresado es inválido o ha expirado."));
        }
        
        var esValido = await repositorioCodigo.ElCodigoEsValidoAsync(request.Codigo, cancellationToken);
        if (!esValido)
        {
            logger.LogWarning("El código con valor {Codigo} ha expirado o no es válido", request.Codigo);
            
            return ResultadoT<string>.Fallo(Error.Fallo("400", "El código ha expirado o no es válido"));
        }
        
        var nuevaContrasena = BCrypt.Net.BCrypt.HashPassword(request.ConfirmacionDeContrsena);

        await repositorioUsuario.ActualizarContrasenaAsync(usuario, nuevaContrasena, cancellationToken);

        logger.LogInformation("La contraseña del usuario con ID '{UsuarioId}' fue actualizada exitosamente.", usuario.Id);

        return ResultadoT<string>.Exito("La contraseña ha sido actualizada correctamente.");
    }
}