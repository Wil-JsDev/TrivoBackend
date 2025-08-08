using Trivo.Dominio.Enum;

namespace Trivo.Aplicacion.DTOs.Notificacion;

public record NotificacionDto
(
    Guid NotificacionId,
    Guid UsuarioId,
    string? Tipo,
    string? Contenido,
    bool? Leido,
    DateTime? FechaCreacion,
    DateTime? FechaLeido
);