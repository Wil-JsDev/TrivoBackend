using System.Data;
using System.Text.Json.Serialization;
using Trivo.Aplicacion.DTOs.Usuario;

namespace Trivo.Aplicacion.DTOs.Mensaje;

public sealed record MensajeDto(
    Guid MensajeId,
    Guid ChatId,
    string? Contenido ,
    string? Estado,
    DateTime? FechaEnvio,
    Guid EmisorId,
    Guid ReceptorId
);
