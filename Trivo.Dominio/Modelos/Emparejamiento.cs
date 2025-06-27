using Trivo.Dominio.Enum;

namespace Trivo.Dominio.Modelos;

public class Emparejamiento : EntidadBase
{
    public Guid? ReclutadorId { get; set; }
    
    public Guid? ExpertoId { get; set; }
    
    public string? ExpertoEstado { get; set; }
    
    public string? ReclutadorEstado { get; set; }
    
    public string? EmparejamientoEstado { get; set; }
    
    public Reclutador? Reclutador { get; set; }
    
    public Experto? Experto { get; set; }
}