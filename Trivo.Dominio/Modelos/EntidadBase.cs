namespace Trivo.Dominio.Modelos;

public class EntidadBase
{
    public Guid? Id { get; set; }

    public DateTime? FechaRegistro { get; set; } = DateTime.UtcNow;

    public DateTime? FechaActualizacion { get; set; }
}