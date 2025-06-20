namespace Trivo.Dominio.Modelos;

public sealed class Habilidad
{
    public Guid? HabilidadId { get; set; }
    
    public string? Nombre { get; set; }
    
    public DateTime? FechaRegistro { get; set; } = DateTime.UtcNow;
    
    public ICollection<UsuarioHabilidad> UsuarioHabilidades { get; set; } = new List<UsuarioHabilidad>();
}