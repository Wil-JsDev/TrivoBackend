using Trivo.Aplicacion.Abstracciones.Mensajes;
using Trivo.Aplicacion.DTOs.Emparejamiento;
using Trivo.Dominio.Enum;

namespace Trivo.Aplicacion.Modulos.Emparejamiento.Commands.Actualizar;

public sealed record ActualizarEmparejamientoCommand
(
    Guid EmparejamientoId,
    Guid UsuarioId,
    FaltaPorEmparejamiento? FaltaPorEmparejamiento,
    EstadoDeActualizacionEmparejamiento? Estado
) : ICommand<EmparejamientoDetallesDto>;