using Trivo.Aplicacion.Abstracciones.Mensajes;

namespace Trivo.Aplicacion.Modulos.Emparejamiento.Commands.Rechazos;

public sealed class CrearRechazosEmparejamientoCommand : ICommand<string>
{
    public Guid? ReclutadorId { get; set; }
    
    public Guid? ExpertoId { get; set; }
    
    public string? CreadoPor { get; set; }
}