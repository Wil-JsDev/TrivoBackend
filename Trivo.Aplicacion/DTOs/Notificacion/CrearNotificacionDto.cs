using Trivo.Dominio.Enum;

namespace Trivo.Aplicacion.DTOs.Notificacion;

public sealed record CrearNotificacionDto
(
    Guid UsuarioId,
    string? TipoNotificacion,
    string? Contenido
);