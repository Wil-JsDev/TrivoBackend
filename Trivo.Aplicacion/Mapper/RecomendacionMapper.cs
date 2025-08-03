using Trivo.Aplicacion.DTOs.Cuentas.Expertos;
using Trivo.Aplicacion.DTOs.Cuentas.Reclutador;
using Trivo.Dominio.Modelos;

namespace Trivo.Aplicacion.Mapper;

public static class RecomendacionMapper
{
    public static ExpertoReconmendacionIaDto MappearAExpertoDto(Usuario usuario)
    {
        var experto = usuario.Expertos?.FirstOrDefault();
        
        return new ExpertoReconmendacionIaDto(
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

    public static ReclutadorReconmendacionIaDto MappearAReclutadorDto(Usuario usuario)
    {
        var reclutador = usuario.Reclutadores?.FirstOrDefault();
        
        return new ReclutadorReconmendacionIaDto(
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