using Trivo.Aplicacion.Abstracciones.Mensajes;
using Trivo.Aplicacion.DTOs.Emparejamiento;

namespace Trivo.Aplicacion.Modulos.Emparejamiento.Commands.Crear;

public record CrearEmparejamientoCommand
(
    Guid? ReclutadorId,
    Guid? ExpertoId
    
) : ICommand<EmparejamientoDto>;