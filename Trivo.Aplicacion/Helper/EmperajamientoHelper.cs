using Trivo.Aplicacion.DTOs.Cuentas.Usuarios;
using Trivo.Aplicacion.DTOs.Emparejamiento;
using Trivo.Aplicacion.Mapper;
using Trivo.Dominio.Enum;
using Trivo.Dominio.Modelos;

namespace Trivo.Aplicacion.Helper;

public static class EmperajamientoHelper
{
    public static EmparejamientoDto EmparejamientoDto(this Emparejamiento emparejamiento, Roles rol)
    {
        UsuarioReconmendacionDto? otroUsuarioDto = null;

        if (rol == Roles.Experto)
        {
            if (emparejamiento.Reclutador?.Usuario != null)
                otroUsuarioDto = UsuarioMapper.MapToDto(emparejamiento.Reclutador.Usuario);
        }
        else if (rol == Roles.Reclutador)
        {
            if (emparejamiento.Experto?.Usuario != null)
                otroUsuarioDto = UsuarioMapper.MapToDto(emparejamiento.Experto.Usuario);
        }

        if (otroUsuarioDto == null)
        {
           Console.WriteLine($"Error - {otroUsuarioDto}");
        }

        return new EmparejamientoDto(
            EmparejamientoId: emparejamiento.Id ?? Guid.Empty,
            UsuarioReconmendacionDto: otroUsuarioDto != null
                ? new List<UsuarioReconmendacionDto> { otroUsuarioDto }
                : Enumerable.Empty<UsuarioReconmendacionDto>()
        );
    }


}