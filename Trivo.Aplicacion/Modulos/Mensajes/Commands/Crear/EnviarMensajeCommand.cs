using Trivo.Aplicacion.Abstracciones.Mensajes;
using Trivo.Aplicacion.DTOs.Mensaje;

namespace Trivo.Aplicacion.Modulos.Mensajes.Commands.Crear;

public record EnviarMensajeCommand
(    
    Guid ChatId,
    Guid EmisorId,
    Guid ReceptorId,
    string Contenido
) : ICommand<MensajeDto>;