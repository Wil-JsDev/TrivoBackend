using Trivo.Aplicacion.DTOs.Cuentas.Usuarios;
using Trivo.Aplicacion.DTOs.Emparejamiento;
using Trivo.Aplicacion.Mapper;
using Trivo.Dominio.Enum;
using Trivo.Dominio.Modelos;

namespace Trivo.Aplicacion.Helper;

public static class EmperajamientoHelper
{
    public static EmparejamientoDto EmparejamientoDto(this Emparejamiento emparejamientos, Roles rol)
    {
        UsuarioReconmendacionDto? otroUsuarioDto = null;

        if ( rol == Roles.Experto && emparejamientos.Reclutador?.Usuario is not null)
        {
            otroUsuarioDto = UsuarioMapper.MapToDto(emparejamientos.Reclutador.Usuario);
        }
        else if ( rol == Roles.Reclutador && emparejamientos.Experto?.Usuario is not null)
        {
            otroUsuarioDto = UsuarioMapper.MapToDto(emparejamientos.Experto.Usuario);
        }

        return new EmparejamientoDto(
            EmparejamientoId: emparejamientos.Id ?? Guid.Empty,
            UsuarioReconmendacionDto: otroUsuarioDto is not null
                ? new List<UsuarioReconmendacionDto> { otroUsuarioDto }
                : Enumerable.Empty<UsuarioReconmendacionDto>()
        );
    }
}