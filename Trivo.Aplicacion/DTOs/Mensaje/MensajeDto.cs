using System.Data;
using System.Text.Json.Serialization;
using Trivo.Aplicacion.DTOs.Usuario;

namespace Trivo.Aplicacion.DTOs.Mensaje;

public record MensajeDto(
    Guid Id,
    Guid ChatId,
    Guid EmisorId,
    string Estado,
    string Contenido,
    DateTime FechaEnvio, 
    [property: JsonIgnore] 
    Guid? ReceptorId =null
);
