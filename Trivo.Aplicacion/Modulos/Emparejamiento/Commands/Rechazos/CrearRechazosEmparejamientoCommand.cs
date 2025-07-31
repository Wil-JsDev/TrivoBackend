using System.Text.Json.Serialization;
using Trivo.Aplicacion.Abstracciones.Mensajes;
using Trivo.Dominio.Enum;

namespace Trivo.Aplicacion.Modulos.Emparejamiento.Commands.Rechazos;

public sealed class CrearRechazosEmparejamientoCommand : ICommand<string>
{
    public Guid? ReclutadorId { get; set; }
    
    public Guid? ExpertoId { get; set; }
    
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public Roles? CreadoPor { get; set; }
}