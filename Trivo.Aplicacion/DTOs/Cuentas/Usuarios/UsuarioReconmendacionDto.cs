using Trivo.Aplicacion.DTOs.Habilidades;
using Trivo.Aplicacion.DTOs.Intereses;

namespace Trivo.Aplicacion.DTOs.Cuentas.Usuarios;

public record UsuarioReconmendacionDto
(
    Guid UsuarioId,
    string? Nombre,
    string? Apellido,
    string? Biografia,
    string? Posicion,
    List<InteresConIdDto> Intereses,
    List<HabilidadConIdDto> Habilidades,
    string? FotoPerfil
);

public sealed record ExpertoReconmendacionDto
(
    Guid ExpertoId,
    Guid UsuarioId,
    string? Nombre,
    string? Apellido,
    string? Ubicacion,
    string? Biografia,
    string? FotoPerfil,
    string? Posicion,
    List<InteresConIdDto> Intereses,
    List<HabilidadConIdDto> Habilidades,
    bool? DisponibleParaProyectos,
    bool? Contratado
) : UsuarioReconmendacionDto(UsuarioId, Nombre, Apellido, Biografia, Posicion, Intereses, Habilidades, FotoPerfil);

public sealed record ReclutadorReconmendacionDto
(
    Guid ReclutadorId,
    Guid UsuarioId,
    string? Nombre,
    string? Apellido,
    string? Ubicacion,
    string? Biografia,
    string? FotoPerfil,
    string? Posicion,
    List<InteresConIdDto> Intereses,
    List<HabilidadConIdDto> Habilidades,
    string? NombreEmpresa
) : UsuarioReconmendacionDto(UsuarioId, Nombre, Apellido, Biografia, Posicion, Intereses, Habilidades, FotoPerfil);