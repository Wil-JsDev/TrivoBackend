using Trivo.Aplicacion.DTOs.Notificacion;
using Trivo.Dominio.Modelos;

namespace Trivo.Aplicacion.Mapper;

public static class NotificacionMapper
{
    public static Notificacion MapearNotificacion(CrearNotificacionDto dto)
    {
        return new Notificacion
        {
            NotificacionId = Guid.NewGuid(),
            UsuarioId = dto.UsuarioId,
            Tipo = dto.Tipo.ToString(),
            Contenido = dto.Contenido,
            Leida = false,
            FechaLeida = DateTime.UtcNow
        };
    }
    public static NotificacionDto MapearNotificacionDto(Notificacion entidad)
    {
        return new NotificacionDto(
            NotificacionId: entidad.NotificacionId ?? Guid.Empty,
            UsuarioId: entidad.UsuarioId ?? Guid.Empty,
            Tipo: entidad.Tipo ?? string.Empty,
            Contenido: entidad.Contenido ?? string.Empty,
            Leido: entidad.Leida ?? false,
            FechaCreacion: entidad.FechaCreacion ?? DateTime.UtcNow,
            FechaLeido: entidad.FechaLeida
        );
    }
    public static List<NotificacionDto> MappearListaADto(IEnumerable<Notificacion> notificaciones)
    {
        return notificaciones?
            .Select(MappearADto)
            .ToList() ?? [];
    }

    #region Metodos Privados

        private static NotificacionDto MappearADto(Notificacion notificacion)
        {
            return new NotificacionDto(
                NotificacionId: notificacion.NotificacionId ?? Guid.Empty,
                UsuarioId: notificacion.UsuarioId ?? Guid.Empty,
                Tipo: notificacion.Tipo,
                Contenido: notificacion.Contenido,
                Leido: notificacion.Leida ?? false,
                FechaCreacion: notificacion.FechaCreacion,
                FechaLeido: notificacion.FechaLeida
            );
        }

    #endregion
    
}