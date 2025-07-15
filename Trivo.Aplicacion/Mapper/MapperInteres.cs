using Trivo.Aplicacion.DTOs.Intereses;
using Trivo.Dominio.Modelos;

namespace Trivo.Aplicacion.Mapper;

public class MapperInteres
{
    
    public static IEnumerable<InteresDto> MapIntereses(IEnumerable<UsuarioInteres>? intereses)
    {
        return intereses?.Select(x => new InteresDto(
            InteresId: x.InteresId ?? Guid.Empty,
            Nombre: x.Interes?.Nombre ?? string.Empty)) ?? [];
    }
}