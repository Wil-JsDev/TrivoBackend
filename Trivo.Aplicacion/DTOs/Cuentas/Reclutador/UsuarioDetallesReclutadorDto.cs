using Trivo.Aplicacion.DTOs.Cuentas.Usuarios;
using Trivo.Aplicacion.DTOs.Habilidades;
using Trivo.Aplicacion.DTOs.Intereses;

namespace Trivo.Aplicacion.DTOs.Cuentas.Reclutador;

public sealed record UsuarioDetallesReclutadorDto(
    string? Nombre,
    string? Apellido,
    string? Ubicacion,
    string? Biografia,
    string? FotoPerfil,
    string? Posicion,
    IEnumerable<HabilidadNombreDto> Habilidad,
    IEnumerable<InteresDto> Interes,
    string? NombreEmpresa
) : UsuarioDetallesDto(Nombre, Apellido, Ubicacion, Biografia, FotoPerfil, Posicion , Habilidad, Interes);
