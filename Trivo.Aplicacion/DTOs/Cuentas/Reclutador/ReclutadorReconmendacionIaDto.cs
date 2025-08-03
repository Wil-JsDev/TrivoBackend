using Trivo.Aplicacion.DTOs.Cuentas.Usuarios;
using Trivo.Aplicacion.DTOs.Habilidades;
using Trivo.Aplicacion.DTOs.Intereses;

namespace Trivo.Aplicacion.DTOs.Cuentas.Reclutador;

public record ReclutadorReconmendacionIaDto
(
    Guid ReclutadorId,
    string? Nombre,
    string? Apellido,
    string? Ubicacion,
    string? Biografia,
    string? FotoPerfil,
    string? Posicion,
    List<InteresConIdDto> Intereses,
    List<HabilidadConIdDto> Habilidades,
    string? NombreEmpresa
) : UsuarioRecomendacionIaDto(Nombre, Apellido, Ubicacion ,Biografia, Posicion, FotoPerfil, Intereses, Habilidades);