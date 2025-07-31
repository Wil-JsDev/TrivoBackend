using System.Text.Json.Serialization;
using Trivo.Aplicacion.Abstracciones.Mensajes;
using Trivo.Aplicacion.DTOs.Emparejamiento;
using Trivo.Dominio.Enum;

namespace Trivo.Aplicacion.Modulos.Emparejamiento.Commands.Crear;

public sealed class CrearEmparejamientoCommand : ICommand<EmparejamientoDetallesDto>
{
    public Guid? ReclutadorId { get; set; }
    public Guid? ExpertoId { get; set; }
    
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public Roles? CreadoPor { get; set; }
}