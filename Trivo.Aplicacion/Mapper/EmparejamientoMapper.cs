using Trivo.Aplicacion.DTOs.Cuentas.Usuarios;
using Trivo.Dominio.Modelos;

namespace Trivo.Aplicacion.Mapper;

public static class EmparejamientoMapper
{
    public static ExpertoReconmendacionDto MappearExpertoReconmendacionDto(Usuario usuario, Experto? experto)
    {
        return new ExpertoReconmendacionDto(
            ExpertoId: experto?.Id ?? Guid.Empty,
            UsuarioId: usuario.Id ?? Guid.Empty,
            Nombre: usuario.Nombre ?? string.Empty,
            Apellido: usuario.Apellido ?? string.Empty,
            Ubicacion: usuario.Ubicacion ?? string.Empty,
            Biografia: usuario.Biografia ?? string.Empty,
            FotoPerfil: usuario.FotoPerfil ?? string.Empty,
            Posicion: usuario.Posicion ?? string.Empty,
            Intereses: UsuarioMapper.MappearAintereses(usuario.UsuarioInteres),
            Habilidades: UsuarioMapper.MappearAHabilidades(usuario.UsuarioHabilidades),
            DisponibleParaProyectos: experto?.DisponibleParaProyectos ?? false,
            Contratado: experto?.Contratado ?? false
        );
    }

    public static ReclutadorReconmendacionDto MappearReclutadorReconmendacionDto(Usuario usuario, Reclutador? reclutador)
    {
        return new ReclutadorReconmendacionDto(
            ReclutadorId: reclutador?.Id ?? Guid.Empty,
            UsuarioId: usuario.Id ?? Guid.Empty,
            Nombre: usuario.Nombre ?? string.Empty,
            Apellido: usuario.Apellido ?? string.Empty,
            Ubicacion: usuario.Ubicacion ?? string.Empty,
            Biografia: usuario.Biografia ?? string.Empty,
            FotoPerfil: usuario.FotoPerfil ?? string.Empty,
            Posicion: usuario.Posicion ?? string.Empty,
            Intereses: UsuarioMapper.MappearAintereses(usuario.UsuarioInteres),
            Habilidades: UsuarioMapper.MappearAHabilidades(usuario.UsuarioHabilidades),
            NombreEmpresa: reclutador?.NombreEmpresa
        );
    }
    
}