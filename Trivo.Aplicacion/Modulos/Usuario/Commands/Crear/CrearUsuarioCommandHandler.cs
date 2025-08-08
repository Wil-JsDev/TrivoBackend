using Microsoft.Extensions.Logging;
using Trivo.Aplicacion.Abstracciones.Mensajes;
using Trivo.Aplicacion.DTOs.Cuentas.Usuarios;
using Trivo.Aplicacion.DTOs.Email;
using Trivo.Aplicacion.Interfaces.Repositorio;
using Trivo.Aplicacion.Interfaces.Repositorio.Cuenta;
using Trivo.Aplicacion.Interfaces.Servicios;
using Trivo.Aplicacion.Interfaces.Servicios.SignaIR;
using Trivo.Aplicacion.Mapper;
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
    IRepositorioUsuarioInteres repositorioUsuarioInteres,
    INotificadorIA notificadorIa
    
    ) : ICommandHandler<CrearUsuarioCommand, UsuarioDto>
{
    public async Task<ResultadoT<UsuarioDto>> Handle(
        CrearUsuarioCommand request, 
        CancellationToken cancellationToken
    )
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
                EstadoUsuario = nameof(EstadoUsuario.Activo),
                Posicion = request.Posicion
            };

            await repositorioUsuario.CrearAsync(usuario, cancellationToken);

            var codigo = await codigoServicio.GenerarCodigoAsync(usuario.Id ?? Guid.Empty, TipoCodigo.ConfirmacionCuenta, cancellationToken);

            if (!codigo.EsExitoso)
            {
                logger.LogError("Falló la generación del código para el usuario '{UsuarioId}'. Error: {Error}",
                    usuario.Id, codigo.Error!.Descripcion);
        
                return ResultadoT<UsuarioDto>.Fallo(codigo.Error!);
            } 
            
            await emailServicio.EnviarEmailAsync(
                new EmailRespuestaDto(
                    Usuario: request.Email!,
                    Cuerpo: EmailTemas.RegistroDeUsuario(usuario.NombreUsuario!, codigo.Valor),
                    Tema: "Confirmar cuenta"
                )
            );
            
            logger.LogInformation("Usuario '{UsuarioId}' creado correctamente.", usuario.Id);
            
            if (request.Intereses is not null && request.Intereses.Any())
            {
                var relacionesInteres = request.Intereses.Select(interesId => new UsuarioInteres
                {
                    UsuarioId = usuario.Id,
                    InteresId = interesId
                }).ToList();

                await repositorioUsuarioInteres.CrearMultiplesUsuarioInteresAsync(relacionesInteres, cancellationToken);
            }
            
            UsuarioDto usuarioDto = new
            (
                UsuarioId: usuario.Id,
                Nombre: usuario.Nombre,
                Apellido: usuario.Apellido,
                Biografia: usuario.Biografia,
                Email: usuario.Email,
                NombreUsuario: usuario.NombreUsuario,
                Ubicacion: usuario.Ubicacion,
                Posicion: usuario.Posicion,
                FotoPerfil: usuario.FotoPerfil,
                EstadoUsuario: usuario.EstadoUsuario,
                FechaRegistro: usuario.FechaRegistro
            );

            var usuarioConRelaciones = await repositorioUsuario.ObtenerPorIdConRelacionesAsync(usuario.Id!.Value, cancellationToken);
            
            await notificadorIa.NotificarNuevaRecomendacion(usuario.Id ?? Guid.Empty, new List<UsuarioRecomendacionIaDto>{ UsuarioMapper.MappearRecomendacionIaDto(usuarioConRelaciones!) });
            
            logger.LogInformation("Se ha notificador correctamente al usuario {UsuarioId}", usuarioConRelaciones!.Id);
            
            return ResultadoT<UsuarioDto>.Exito(usuarioDto);
    }
}