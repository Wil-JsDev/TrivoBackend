using System.Data;
using System.Text.Json.Serialization;
using Trivo.Aplicacion.DTOs.Usuario;

namespace Trivo.Aplicacion.DTOs.Mensaje;

public record MensajeDto(
    Guid MensajeId,
    Guid ChatId,
    string? Contenido ,
    string? Estado,
    DateTime? FechaEnvio ,
    Guid EmisorId,
    UsuarioDto? Emisor,
    Guid ReceptorId,
    UsuarioDto? Receptor
);
