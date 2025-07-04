using Microsoft.Extensions.Logging;
using Trivo.Aplicacion.Abstracciones.Mensajes;
using Trivo.Aplicacion.DTOs.Cuentas.Usuarios;
using Trivo.Aplicacion.DTOs.Email;
using Trivo.Aplicacion.Interfaces.Repositorio;
using Trivo.Aplicacion.Interfaces.Repositorio.Cuenta;
using Trivo.Aplicacion.Interfaces.Servicios;
using Trivo.Aplicacion.Utilidades;
using Trivo.Dominio.Enum;
using Trivo.Dominio.Modelos;

namespace Trivo.Aplicacion.Modulos.Usuario.Commands.Crear;

internal sealed class CrearUsuarioCommandHandler(
    IRepositorioUsuario repositorioUsuario,
    IEmailServicio emailServicio,
    ICodigoServicio codigoServicio,
    ICloudinaryServicio cloudinaryServicio,
    ILogger<CrearUsuarioCommandHandler> logger,
    IRepositorioCategoriaInteres categoriaInteresRepositorio,
    IRepositorioUsuarioInteres repositorioUsuarioInteres
    
    ) : ICommandHandler<CrearUsuarioCommand, UsuarioDto>
{
    public async Task<ResultadoT<UsuarioDto>> Handle(
        CrearUsuarioCommand request, 
        CancellationToken cancellationToken
    )
    {

        if (request is null)
        {
            // Validar email duplicado
            if (await repositorioUsuario.ExisteEmailAsync(request!.Email!, cancellationToken))
            {
                logger.LogWarning("El email '{Email}' ya está en uso.", request.Email);

                return ResultadoT<UsuarioDto>.Fallo(Error.Conflicto("409", "Ya ha sido tomado este email"));
            }

            // Validar nombre de usuario duplicado
            if (await repositorioUsuario.ExisteNombreUsuarioAsync(request.NombreUsuario!, cancellationToken))
            {
                logger.LogWarning("El nombre de usuario '{NombreUsuario}' ya está en uso.", request.NombreUsuario);

                return ResultadoT<UsuarioDto>.Fallo(Error.Conflicto("409", "Ya ha sido tomado este nombre de usuario"));
            }

            string imageUrl = "";
            if (request.Foto != null)
            {
                using var stream = request.Foto.OpenReadStream();
                imageUrl = await cloudinaryServicio.SubirImagenAsync(
                    stream,
                    request.Foto.FileName,
                    cancellationToken);
            }

            Dominio.Modelos.Usuario usuario = new()
            {
                Id = Guid.NewGuid(),
                Nombre = request.Nombre,
                Apellido = request.Apellido,
                Biografia = request.Biografia,
                CuentaConfirmada = false,
                Email = request.Email,
                ContrasenaHash = BCrypt.Net.BCrypt.HashPassword(request.Contrasena),
                NombreUsuario = request.NombreUsuario,
                Ubicacion = request.Ubicacion,
                FotoPerfil = imageUrl,
                EstadoUsuario = nameof(EstadoUsuario.Activo)
            };

            await repositorioUsuario.CrearAsync(usuario, cancellationToken);

            var codigo = await codigoServicio.GenerarCodigoAsync(usuario.Id ?? Guid.Empty, cancellationToken);

            await emailServicio.EnviarEmailAsync(
                new EmailRespuestaDto(
                    Usuario: request.Email!,
                    Cuerpo: EmailTemas.RegistroDeUsuario(usuario.NombreUsuario!, codigo.Valor),
                    Tema: "Confirmar cuenta"
                )
            );
            
            logger.LogInformation("Usuario '{UsuarioId}' creado correctamente.", usuario.Id);
            
            var relacionesInteres = request.Intereses!.Select(interesId => new UsuarioInteres
            {
                UsuarioId = usuario.Id,
                InteresId = interesId
            }).ToList();
            
            await repositorioUsuarioInteres.CrearMultiplesUsuarioInteresAsync(relacionesInteres, cancellationToken); // This 
            
            UsuarioDto usuarioDto = new
            (
                UsuarioId: usuario.Id,
                Nombre: usuario.Nombre,
                Apellido: usuario.Apellido,
                Biografia: usuario.Biografia,
                Email: usuario.Email,
                NombreUsuario: usuario.NombreUsuario,
                Ubicacion: usuario.Ubicacion,
                FotoPerfil: usuario.FotoPerfil,
                EstadoUsuario: usuario.EstadoUsuario,
                FechaRegistro: usuario.FechaRegistro
            );

            return ResultadoT<UsuarioDto>.Exito(usuarioDto);
        }
        
        logger.LogWarning("Se recibió un comando CrearUsuarioCommand nulo.");

        return ResultadoT<UsuarioDto>.Fallo(Error.Fallo("400", "La solicitud no puede ser nula."));
    }
}