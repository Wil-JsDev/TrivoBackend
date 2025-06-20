using Trivo.Dominio.Enum;

namespace Trivo.Dominio.Modelos;

public class Emparejamiento : EntidadBase
{
    public Guid? ReclutadorId { get; set; }
    
    public Guid? ExpertoId { get; set; }
    
    public ExpertoEstado? ExpertoEstado { get; set; }
    
    public ReclutadorEstado? ReclutadorEstado { get; set; }
    
    public EmparejamientoEstado? EmparejamientoEstado { get; set; }
    
    public Reclutador? Reclutador { get; set; }
    
    public Experto? Experto { get; set; }
}