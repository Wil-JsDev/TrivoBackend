using Microsoft.Extensions.Logging;
using Trivo.Aplicacion.Abstracciones.Mensajes;
using Trivo.Aplicacion.Interfaces.Repositorio.Cuenta;
using Trivo.Aplicacion.Interfaces.Servicios;
using Trivo.Aplicacion.Utilidades;

namespace Trivo.Aplicacion.Modulos.Usuario.Commands.ActualizarImagen;

internal sealed class ActualizarImagenUsuarioCommandHandler(
    IRepositorioUsuario repositorioUsuario,
    ILogger<ActualizarImagenUsuarioCommandHandler> logger,
    ICloudinaryServicio  cloudinaryServicio
    ) : ICommandHandler<ActualizarImagenUsuarioCommand, string>
{
    public async Task<ResultadoT<string>> Handle(
        ActualizarImagenUsuarioCommand request, 
        CancellationToken cancellationToken
    )
    {
        if (request == null)
        {
            logger.LogWarning("Se recibió una solicitud nula en el comando correspondiente.");

            return ResultadoT<string>.Fallo(Error.Fallo("400", "La solicitud no puede ser nula."));
        }
        
        var usuario = await repositorioUsuario.ObtenerByIdAsync(request.UsuarioId, cancellationToken);
        if (usuario == null)
        {
            logger.LogWarning("No se encontró el usuario con ID '{RequestUsuarioId}'.", request.UsuarioId);

            return ResultadoT<string>.Fallo(Error.NoEncontrado("404", "El usuario no fue encontrado."));
        }

        if (request.Imagen!.Length == 0)
        {
            logger.LogWarning("La imagen enviada está vacía o es inválida para el usuario con ID '{UserId}'.", request.UsuarioId);
        
            return ResultadoT<string>.Fallo(Error.Fallo("400", "La imagen es obligatoria."));
        }

        await using var stream = request.Imagen.OpenReadStream();

        var imagen = await cloudinaryServicio.SubirImagenAsync(
            stream,
            request.Imagen.FileName,
            cancellationToken
        );

        usuario.FotoPerfil = imagen;

        await repositorioUsuario.ActualizarAsync(usuario, cancellationToken);

        logger.LogInformation("Imagen de perfil actualizada correctamente para el usuario con ID '{UsuarioId}'.", usuario.Id);

        return ResultadoT<string>.Exito("La imagen de perfil ha sido actualizada correctamente.");
    }
}