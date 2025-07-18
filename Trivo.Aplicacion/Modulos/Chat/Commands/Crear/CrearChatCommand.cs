using Trivo.Aplicacion.Abstracciones.Mensajes;
using Trivo.Aplicacion.DTOs.Chat;

namespace Trivo.Aplicacion.Modulos.Chat.Commands.Crear;

public record CrearChatCommand(
    Guid EmisorId,
    Guid ReceptorId
    ): ICommand<ChatDto>;