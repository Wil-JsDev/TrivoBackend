using System.Text.Json;
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
        Guid? expertoId = null;
        Guid? reclutadorId = null;
        UsuarioRecomendacionIaDto? otroUsuarioDto = null;

        if (rol == Roles.Experto && emparejamiento.Reclutador?.Usuario != null)
        {
            reclutadorId = emparejamiento.Reclutador.Id;
            otroUsuarioDto = RecomendacionMapper.MappearAReclutadorDto(
                emparejamiento.Reclutador!.Usuario!,
                emparejamiento.Reclutador
            );

            Console.WriteLine($"[DEBUG - EXPERTO] ReclutadorId: {reclutadorId}");
            Console.WriteLine($"[DEBUG - EXPERTO] Usuario recomendado: {JsonSerializer.Serialize(otroUsuarioDto, new JsonSerializerOptions { WriteIndented = true })}");
        }
        else if (rol == Roles.Reclutador && emparejamiento.Experto?.Usuario != null)
        {
            expertoId = emparejamiento.Experto.Id;
            otroUsuarioDto = RecomendacionMapper.MappearAExpertoDto(
                emparejamiento.Experto!.Usuario!,
                emparejamiento.Experto
            );

            Console.WriteLine($"[DEBUG - RECLUTADOR] ExpertoId: {expertoId}");
            Console.WriteLine($"[DEBUG - RECLUTADOR] Usuario recomendado: {JsonSerializer.Serialize(otroUsuarioDto, new JsonSerializerOptions { WriteIndented = true })}");
        }
        else
        {
            Console.WriteLine("[DEBUG] Rol no coincide o el otro usuario no está presente.");
        }

        // return new EmparejamientoDto(
        //     EmparejamientoId: emparejamiento.Id ?? Guid.Empty,
        //     ReclutadotId: reclutadorId,
        //     ExpertoId: expertoId,
        //     ExpertoEstado: emparejamiento.ExpertoEstado,
        //     ReclutadorEstado: emparejamiento.ReclutadorEstado,
        //     EmparejamientoEstado: emparejamiento.EmparejamientoEstado,
        //     FechaRegistro: emparejamiento.FechaRegistro,
        //     UsuarioReconmendacionDto: otroUsuarioDto
        // );

        return null;
    }




}