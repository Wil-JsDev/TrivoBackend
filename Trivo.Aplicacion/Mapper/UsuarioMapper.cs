using Trivo.Aplicacion.DTOs.Cuentas.Usuarios;
using Trivo.Aplicacion.DTOs.Habilidades;
using Trivo.Aplicacion.DTOs.Intereses;

namespace Trivo.Aplicacion.Mapper;

public static class UsuarioMapper
{
    public static UsuarioReconmendacionDto MapToDto(Dominio.Modelos.Usuario entidad)
    {
        return new UsuarioReconmendacionDto(
            UsuarioId: entidad.Id ?? Guid.Empty,
            Nombre: entidad.Nombre,
            Apellido: entidad.Apellido,
            Biografia: entidad.Biografia,
            Intereses: entidad.UsuarioInteres?.Select(i => new InteresConIdDto(i.Interes?.Id ?? Guid.Empty, i.Interes?.Nombre ?? "")).ToList() ?? new List<InteresConIdDto>(),
            Habilidades: entidad.UsuarioHabilidades?.Select(h => new HabilidadConIdDto(h.Habilidad?.HabilidadId ?? Guid.Empty, h.Habilidad?.Nombre ?? "")).ToList() ?? new List<HabilidadConIdDto>(),
            FotoPerfil: entidad.FotoPerfil
        );
    }
}