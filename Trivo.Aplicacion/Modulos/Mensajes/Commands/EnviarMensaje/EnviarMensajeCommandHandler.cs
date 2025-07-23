using MediatR;
using Microsoft.Extensions.Logging;
using Trivo.Aplicacion.Abstracciones.Mensajes;
using Trivo.Aplicacion.DTOs.Emparejamiento;
using Trivo.Aplicacion.DTOs.Mensaje;
using Trivo.Aplicacion.DTOs.Usuario;
using Trivo.Aplicacion.Interfaces.Repositorio;
using Trivo.Aplicacion.Interfaces.Repositorio.Cuenta;
using Trivo.Aplicacion.Interfaces.Servicios.SignaIR;
using Trivo.Aplicacion.Utilidades;
using Trivo.Dominio.Enum;
using Trivo.Dominio.Modelos;

namespace Trivo.Aplicacion.Modulos.Mensajes.Commands.Crear;

internal sealed class EnviarMensajeCommandHandler( 
    ILogger<EnviarMensajeCommandHandler> logger,
    IRepositorioMensaje repositorioMensaje,
    IRepositorioChat repositorioChat,
    INotificadorTiempoReal notificador
    ): ICommandHandler<EnviarMensajeCommand, MensajeDto>
{
    public async Task<ResultadoT<MensajeDto>> Handle(EnviarMensajeCommand request, CancellationToken cancellationToken)
    {
        if (request is null)
        {
            logger.LogWarning("La solicitud enviada no puede ser nula.");
            
            return ResultadoT<MensajeDto>.Fallo(Error.Fallo("400", "La solicitud enviada no puede ser nula."));
        }
        
        var chatExiste = await repositorioChat.ExisteAsync(request.ChatId, cancellationToken);
        
        if (!chatExiste)
        {
            logger.LogWarning("Chat no existe: {ChatId}", request.ChatId);
            return ResultadoT<MensajeDto>.Fallo(Error.Fallo("404", "El chat no existe"));
        }
        
        var emisorPertenece = await repositorioChat.UsuarioPerteneceAlChatAsync(request.ChatId,request.EmisorId, cancellationToken);
        var receptorPertenece = await repositorioChat.UsuarioPerteneceAlChatAsync(request.ChatId,request.ReceptorId, cancellationToken);
        
        var emisor = await repositorioChat.ObtenerUsuarioPorIdAsync(request.EmisorId, cancellationToken);
        var receptor = await repositorioChat.ObtenerUsuarioPorIdAsync(request.ReceptorId, cancellationToken);

        if (emisor == null || receptor == null)
        {
            logger.LogWarning("Emisor o receptor no encontrados");
            return ResultadoT<MensajeDto>.Fallo(Error.Fallo("404", "Emisor o receptor no encontrados"));
        }
        
        if (!emisorPertenece || !receptorPertenece)
        {
            logger.LogWarning("El emisor o receptor no pertenece al chat {ChatId}", request.ChatId);
            return ResultadoT<MensajeDto>.Fallo(Error.Fallo("403", "Emisor o receptor no pertenece al chat"));
        
        }
        
        if (string.IsNullOrEmpty(request.Contenido))
        {
            logger.LogWarning("Contenido del mensaje vacio");
            return ResultadoT<MensajeDto>.Fallo(Error.Fallo("400", "El contenido del mensaje es obligatorio"));
        }
        
        var mensaje = new Mensaje
        {
            MensajeId = Guid.NewGuid(),
            ChatId = request.ChatId,
            EmisorId = request.EmisorId,
            ReceptorId = request.ReceptorId, 
            Contenido = request.Contenido,
            FechaEnvio = DateTime.UtcNow,
            FechaRegistro = DateTime.UtcNow,
            Estado = EstadoMensaje.Enviado.ToString()
        };

        await repositorioMensaje.CrearAsync(mensaje, cancellationToken);

        var dto = new MensajeDto(
            mensaje.MensajeId.Value,
            mensaje.ChatId.Value,
            mensaje.Contenido,
            mensaje.Estado,
            mensaje.FechaEnvio ?? DateTime.UtcNow,
            mensaje.EmisorId.Value,
            new UsuarioDto(
                emisor.Id!.Value,
                emisor.Nombre,
                emisor.Apellido,
                emisor.FotoPerfil
            ),
            mensaje.ReceptorId,
            new UsuarioDto(
                receptor.Id!.Value,
                receptor.Nombre,
                receptor.Apellido,
                receptor.FotoPerfil
            )
        );
        
        await notificador.NotificarMensajePrivado(dto, dto.EmisorId);
        await notificador.NotificarMensajePrivado(dto, dto.ReceptorId);
        logger.LogInformation("Mensaje enviado de {EmisorId} a {ReceptorId}", request.EmisorId);

        return ResultadoT<MensajeDto>.Exito(dto);
    }
}