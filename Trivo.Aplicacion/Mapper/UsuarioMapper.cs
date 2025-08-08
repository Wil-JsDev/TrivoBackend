using Trivo.Aplicacion.DTOs.Cuentas.Usuarios;
using Trivo.Aplicacion.DTOs.Habilidades;
using Trivo.Aplicacion.DTOs.Intereses;
using Trivo.Dominio.Enum;
using Trivo.Dominio.Modelos;

namespace Trivo.Aplicacion.Mapper;

public static class UsuarioMapper
{
    public static UsuarioReconmendacionDto? MapToDto(Dominio.Modelos.Usuario entidad)
    {
        return new UsuarioReconmendacionDto(
            UsuarioId: entidad.Id ?? Guid.Empty,
            Nombre: entidad.Nombre,
            Apellido: entidad.Apellido,
            Biografia: entidad.Biografia,
            Posicion: entidad.Posicion,
            Intereses: entidad.UsuarioInteres?.Select(i => new InteresConIdDto(i.Interes?.Id ?? Guid.Empty, i.Interes?.Nombre ?? "")).ToList() ?? new List<InteresConIdDto>(),
            Habilidades: entidad.UsuarioHabilidades?.Select(h => new HabilidadConIdDto(h.Habilidad?.HabilidadId ?? Guid.Empty, h.Habilidad?.Nombre ?? "")).ToList() ?? new List<HabilidadConIdDto>(),
            FotoPerfil: entidad.FotoPerfil
        );
    }

    public static UsuarioRecomendacionIaDto MappearRecomendacionIaDto(Usuario entidad)
    {
        return new UsuarioRecomendacionIaDto(
            UsuarioId: entidad.Id ?? Guid.Empty,
            Nombre: entidad.Nombre,
            Apellido: entidad.Apellido,
            Ubicacion: entidad.Ubicacion,
            Biografia: entidad.Biografia,
            Posicion: entidad.Posicion,
            FotoPerfil: entidad.FotoPerfil,
            Intereses: MappearAintereses( entidad.UsuarioInteres ),
            Habilidades: MappearAHabilidades( entidad.UsuarioHabilidades )
        );
    }
    public static List<UsuarioRecomendacionIaDto> MappearListaARecomendacionDto(IEnumerable<Usuario> usuarios)
    {
        return usuarios.Select(MappearRecomendacionIaDto).ToList();
    }
    
    public static UsuarioDto MapUsuarioDto(Dominio.Modelos.Usuario usuario)
    {
        return new UsuarioDto
        (
            UsuarioId: usuario.Id,
            Nombre: usuario.Nombre,
            Apellido: usuario.Apellido,
            Biografia: usuario.Biografia,
            Email: usuario.Email,
            NombreUsuario: usuario.NombreUsuario,
            Ubicacion: usuario.Ubicacion,
            Posicion: usuario.Posicion,
            FotoPerfil: usuario.FotoPerfil,
            EstadoUsuario: usuario.EstadoUsuario,
            FechaRegistro: usuario.FechaRegistro
        );
    }
    
        public static List<InteresConIdDto> MappearAintereses(ICollection<UsuarioInteres>? usuarioIntereses)
        {
            return usuarioIntereses?
                .Where(ui => ui.Interes != null)
                .Select(ui => new InteresConIdDto(
                    ui.Interes!.Id ?? Guid.Empty,
                    ui.Interes.Nombre ?? string.Empty))
                .ToList() ?? new List<InteresConIdDto>();
        }

        public static List<HabilidadConIdDto> MappearAHabilidades(ICollection<UsuarioHabilidad>? usuarioHabilidades)
        {
            return usuarioHabilidades?
                .Where(uh => uh.Habilidad != null)
                .Select(uh => new HabilidadConIdDto(
                    uh.Habilidad!.HabilidadId ?? Guid.Empty,
                    uh.Habilidad.Nombre ?? string.Empty))
                .ToList() ?? new List<HabilidadConIdDto>();
        }
    
}
