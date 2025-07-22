using Microsoft.Extensions.Logging;
using Trivo.Aplicacion.Abstracciones.Mensajes;
using Trivo.Aplicacion.DTOs.Chat;
using Trivo.Aplicacion.DTOs.Mensaje;
using Trivo.Aplicacion.DTOs.Usuario;
using Trivo.Aplicacion.Interfaces.Repositorio;
using Trivo.Aplicacion.Interfaces.Repositorio.Cuenta;
using Trivo.Aplicacion.Interfaces.Servicios.SignaIR;
using Trivo.Aplicacion.Mapper;
using Trivo.Aplicacion.Utilidades;
using Trivo.Dominio.Enum;

namespace Trivo.Aplicacion.Modulos.Chat.Commands.Crear;

internal class CrearChatCommandHandler(
    ILogger<CrearChatCommandHandler> logger,
    IRepositorioChat repositorioChat,
    IRepositorioUsuario repositorioUsuario,
    INotificadorTiempoReal notificador
    ) : ICommandHandler<CrearChatCommand, ChatDto>
{
    public async Task<ResultadoT<ChatDto>> Handle(CrearChatCommand request, CancellationToken cancellationToken)
    {
        if (request is null)
        {
            logger.LogWarning("El comando CrearChatCommand es nulo");
            return ResultadoT<ChatDto>.Fallo(Error.NoEncontrado("404", "El comando no puede ser nulo"));
        }
        
        if (request.EmisorId == request.ReceptorId)
        {
            return ResultadoT<ChatDto>.Fallo(Error.Fallo("400", "El emisor y receptor no pueden ser el mismo usuario."));
        }

        var chatExistente = await repositorioChat.BuscarChat1a1Async(request.EmisorId, request.ReceptorId, cancellationToken);
            

        if (chatExistente is not null)
        {
            logger.LogInformation("Ya existe un chat entre {EmisorId} y {ReceptorId}", request.EmisorId, request.ReceptorId);
            return ResultadoT<ChatDto>.Exito(MapperChat.MapChatToDto(chatExistente, request.EmisorId ));
        }
        
        
        var emisor = await repositorioUsuario.ObtenerByIdAsync(request.EmisorId, cancellationToken);
        if (emisor is null)
        {
            return ResultadoT<ChatDto>.Fallo(Error.NoEncontrado("404", "Usuario emisor no encontrado"));
        }        
        
        var receptor = await repositorioUsuario.ObtenerByIdAsync(request.ReceptorId, cancellationToken);

        if (receptor is null)
        {
            return ResultadoT<ChatDto>.Fallo(Error.NoEncontrado("404", "Usuario receptor no encontrado"));
        }

        
        var nuevoChat = new Dominio.Modelos.Chat
        {
            Id = Guid.NewGuid(),
            TipoChat = TipoChat.Privado.ToString(),
            Activo = true,
            FechaRegistro = DateTime.UtcNow,
            ChatUsuarios = new List<Dominio.Modelos.ChatUsuario>
            {
                new() { 
                    UsuarioId = emisor.Id, 
                    FechaIngreso = DateTime.UtcNow, 
                    Usuario = emisor,
                    NombreChat = $"{receptor.Nombre} {receptor.Apellido}".Trim()  
                },
                new() { 
                    UsuarioId = receptor.Id, 
                    FechaIngreso = DateTime.UtcNow, 
                    Usuario = receptor,
                    NombreChat = $"{emisor.Nombre} {emisor.Apellido}".Trim() 
                }
            }
        };
        var resultado = MapperChat.MapChatToDto(nuevoChat, request.EmisorId);

        await notificador.NotificarNuevoChat(request.EmisorId, new List<ChatDto> { resultado });
        await notificador.NotificarNuevoChat(request.ReceptorId, new List<ChatDto> { resultado });
        await repositorioChat.CrearAsync(nuevoChat, cancellationToken);
        
        logger.LogInformation("Se creó un nuevo chat entre {EmisorId} y {ReceptorId}", request.EmisorId, request.ReceptorId);
        return ResultadoT<ChatDto>.Exito(MapperChat.MapChatToDto(nuevoChat, request.EmisorId));
    }
    
    
}