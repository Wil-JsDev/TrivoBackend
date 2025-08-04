using Trivo.Aplicacion.DTOs.Cuentas.Usuarios;
using Trivo.Aplicacion.DTOs.Habilidades;
using Trivo.Aplicacion.DTOs.Intereses;
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
            Intereses: usuario.UsuarioInteres?.Select(ui => new InteresConIdDto(
                InteresId:ui.Interes?.Id ?? Guid.Empty,
                Nombre: ui.Interes?.Nombre ?? string.Empty
            )).ToList() ?? new List<InteresConIdDto>(),
            
            Habilidades: usuario.UsuarioHabilidades?.Select(uh => new HabilidadConIdDto(
                HabilidadId: uh.Habilidad!.HabilidadId ?? Guid.Empty,
                Nombre: uh.Habilidad?.Nombre ?? string.Empty
            )).ToList() ?? new List<HabilidadConIdDto>(),
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
            Intereses: usuario.UsuarioInteres?.Select(ui => new InteresConIdDto(
                InteresId:ui.Interes?.Id ?? Guid.Empty,
                Nombre: ui.Interes?.Nombre ?? string.Empty
            )).ToList() ?? new List<InteresConIdDto>(),
            
            Habilidades: usuario.UsuarioHabilidades?.Select(uh => new HabilidadConIdDto(
                HabilidadId: uh.Habilidad!.HabilidadId ?? Guid.Empty,
                Nombre: uh.Habilidad?.Nombre ?? string.Empty
            )).ToList() ?? new List<HabilidadConIdDto>(),
            NombreEmpresa: reclutador?.NombreEmpresa
        );
    }
    
}