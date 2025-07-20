using Trivo.Aplicacion.Abstracciones.Mensajes;
using Trivo.Aplicacion.DTOs.Emparejamiento;

namespace Trivo.Aplicacion.Modulos.Emparejamiento.Commands.Crear;

public sealed class CrearEmparejamientoCommand : ICommand<EmparejamientoDetallesDto>
{
    public Guid? ReclutadorId { get; set; }
    public Guid? ExpertoId { get; set; }
    public string? CreadoPor { get; set; }
}