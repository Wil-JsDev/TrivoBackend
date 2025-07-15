using Trivo.Aplicacion.DTOs.Habilidades;
using Trivo.Dominio.Modelos;

namespace Trivo.Aplicacion.Mapper;

public class HabilidadesMapper
{
    public static IEnumerable<HabilidadNombreDto> MapHabilidades(IEnumerable<UsuarioHabilidad>? habilidades)
    {
        return habilidades?.Select(x => new HabilidadNombreDto(
            HabilidadId: x.HabilidadId ?? Guid.Empty,
            Nombre: x.Habilidad?.Nombre ?? string.Empty)) ?? [];
    }

}