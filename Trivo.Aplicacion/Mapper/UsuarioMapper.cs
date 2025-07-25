using Trivo.Aplicacion.DTOs.Cuentas.Usuarios;
using Trivo.Aplicacion.DTOs.Habilidades;
using Trivo.Aplicacion.DTOs.Intereses;
using Trivo.Dominio.Enum;

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
            Posicion: entidad.Posicion,
            Intereses: entidad.UsuarioInteres?.Select(i => new InteresConIdDto(i.Interes?.Id ?? Guid.Empty, i.Interes?.Nombre ?? "")).ToList() ?? new List<InteresConIdDto>(),
            Habilidades: entidad.UsuarioHabilidades?.Select(h => new HabilidadConIdDto(h.Habilidad?.HabilidadId ?? Guid.Empty, h.Habilidad?.Nombre ?? "")).ToList() ?? new List<HabilidadConIdDto>(),
            FotoPerfil: entidad.FotoPerfil
        );
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
    
    
    
}