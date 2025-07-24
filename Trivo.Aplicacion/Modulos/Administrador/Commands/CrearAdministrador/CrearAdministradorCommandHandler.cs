using Microsoft.Extensions.Logging;
using Trivo.Aplicacion.Abstracciones.Mensajes;
using Trivo.Aplicacion.DTOs.Cuentas.Administrador;
using Trivo.Aplicacion.Interfaces.Repositorio.Cuenta;
using Trivo.Aplicacion.Interfaces.Servicios;
using Trivo.Aplicacion.Utilidades;

namespace Trivo.Aplicacion.Modulos.Administrador.Commands.CrearAdministrador;

internal sealed class CrearAdministradorCommandHandler(
    ILogger<CrearAdministradorCommandHandler> logger,
    IRepositorioAdministrador repositorioAdministrador,
    ICloudinaryServicio cloudinaryServicio
    ) : ICommandHandler<CrearAdministradorCommand, AdministradorDto>
{
    public async Task<ResultadoT<AdministradorDto>> Handle(CrearAdministradorCommand request, CancellationToken cancellationToken)
    {
        if ( await repositorioAdministrador.ExisteEmailAsync(request.Email, cancellationToken) )
        {
            logger.LogWarning("Intento de creación fallido: el email '{Email}' ya está en uso.", request.Email);
            
            return ResultadoT<AdministradorDto>.Fallo(Error.Conflicto("409", "El email proporcionado ya está registrado."));
        }

        if (await repositorioAdministrador.ExisteNombreUsuarioAsync(request.NombreUsuario, cancellationToken))
        {
            logger.LogWarning("Intento de creación fallido: el nombre de usuario '{NombreUsuario}' ya está en uso.", request.NombreUsuario);
            
            return ResultadoT<AdministradorDto>.Fallo(Error.Conflicto("409", "El nombre de usuario ya se encuentra registrado."));
        }

        string imageUrl = "";
        if (request.Foto != null)
        {
            using var stream = request.Foto.OpenReadStream();
            imageUrl = await cloudinaryServicio.SubirImagenAsync(
                stream,
                request.Foto.FileName,
                cancellationToken);
            logger.LogInformation("Imagen de perfil subida correctamente para el administrador con email '{Email}'.", request.Email);
        }

        Dominio.Modelos.Administrador administrador = new()
        {
            Id = Guid.NewGuid(),
            Nombre = request.Nombre,
            Apellido = request.Apellido,
            Biografia = request.Biografia,
            Email = request.Email,
            ContrasenaHash = BCrypt.Net.BCrypt.HashPassword(request.Contrasena),
            NombreUsuario = request.NombreUsuario,
            FotoPerfil = imageUrl
        };

        await repositorioAdministrador.CrearAsync(administrador, cancellationToken);
        logger.LogInformation("Administrador creado exitosamente con ID '{Id}' y nombre de usuario '{NombreUsuario}'.", administrador.Id, administrador.NombreUsuario);
        
        logger.LogInformation("Correo de confirmación enviado al administrador con email '{Email}'.", administrador.Email);

        AdministradorDto administradorDto = new
        (
            AdministradorId: administrador.Id,
            Nombre: administrador.Nombre,
            Apellido: administrador.Apellido,
            Biografia: administrador.Biografia,
            Email: administrador.Email,
            NombreUsuario: administrador.NombreUsuario,
            FotoPerfil: administrador.FotoPerfil,
            FechaRegistro: administrador.FechaRegistro
        );

        logger.LogInformation("Proceso de creación de administrador completado correctamente para el usuario '{NombreUsuario}'.", administrador.NombreUsuario);

        return ResultadoT<AdministradorDto>.Exito(administradorDto);
    }

}