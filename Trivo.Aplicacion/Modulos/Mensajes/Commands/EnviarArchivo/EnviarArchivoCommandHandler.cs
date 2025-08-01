using Microsoft.Extensions.Logging;
using Trivo.Aplicacion.Abstracciones.Mensajes;
using Trivo.Aplicacion.DTOs.Mensaje;
using Trivo.Aplicacion.DTOs.Usuario;
using Trivo.Aplicacion.Interfaces.Repositorio;
using Trivo.Aplicacion.Interfaces.Servicios;
using Trivo.Aplicacion.Interfaces.Servicios.SignaIR;
using Trivo.Aplicacion.Modulos.Mensajes.Commands.EnviarImagen;
using Trivo.Aplicacion.Utilidades;
using Trivo.Dominio.Enum;
using Trivo.Dominio.Modelos;

namespace Trivo.Aplicacion.Modulos.Mensajes.Commands.EnviarArchivo;

internal class EnviarArchivoCommandHandler(
    ILogger<EnviarImagenCommandHandler> logger,
    ICloudinaryServicio cloudinaryServicio,
    IRepositorioChat repositorioChat,
    IRepositorioMensaje repositorioMensaje,
    INotificadorTiempoReal notificador
): ICommandHandler<EnviarArchivoCommand, MensajeDto>
{
    public async Task<ResultadoT<MensajeDto>> Handle(EnviarArchivoCommand request, CancellationToken cancellationToken)
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

        if (!emisorPertenece || !receptorPertenece)
        {
            logger.LogWarning("El emisor o receptor no pertenece al chat {ChatId}", request.ChatId);
            return ResultadoT<MensajeDto>.Fallo(Error.Fallo("403", "Emisor o receptor no pertenece al chat"));
        
        }
        var emisor = await repositorioChat.ObtenerUsuarioPorIdAsync(request.EmisorId, cancellationToken);
        var receptor = await repositorioChat.ObtenerUsuarioPorIdAsync(request.ReceptorId, cancellationToken);

        if (emisor == null || receptor == null)
        {
            logger.LogWarning("Emisor o receptor no encontrados");
            return ResultadoT<MensajeDto>.Fallo(Error.Fallo("404", "Emisor o receptor no encontrados"));
        }
        
        string url = "";
        if (request.Archivo != null)
        {
            using var stream = request.Archivo.OpenReadStream();
            url = await cloudinaryServicio.SubirArchivoAsync(
                stream,
                request.Archivo.FileName,
                cancellationToken);
        }
        logger.LogInformation("Archivo subido exitosamente a Cloudinary. URL: {Url}", url);



        var mensaje = new Mensaje
        {
            MensajeId = Guid.NewGuid(),
            ChatId = request.ChatId,
            EmisorId = request.EmisorId,
            ReceptorId = request.ReceptorId, 
            Contenido = url,
            FechaEnvio = DateTime.UtcNow,
            FechaRegistro = DateTime.UtcNow,
            Estado = EstadoMensaje.Enviado.ToString()
        };
        logger.LogInformation("Mensaje persistido en la base de datos. Id: {MensajeId}", mensaje.MensajeId);

        await repositorioMensaje.CrearAsync(mensaje, cancellationToken);

        var dto = new MensajeDto(
            mensaje.MensajeId.Value,
            mensaje.ChatId.Value,
            mensaje.Contenido,
            mensaje.Estado,
            mensaje.FechaEnvio ?? DateTime.UtcNow,
            mensaje.EmisorId.Value,
            mensaje.ReceptorId
         
        );
        
        await notificador.NotificarMensajePrivado(dto, dto.EmisorId);
        await notificador.NotificarMensajePrivado(dto, dto.ReceptorId);
        
        logger.LogInformation("Mensaje enviado de {EmisorId} a {ReceptorId}", request.EmisorId);

        return ResultadoT<MensajeDto>.Exito(dto);
    }
    
}