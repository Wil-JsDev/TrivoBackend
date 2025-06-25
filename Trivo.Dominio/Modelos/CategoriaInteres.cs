namespace Trivo.Dominio.Modelos;

public sealed class CategoriaInteres
{
    public Guid? CategoriaId { get; set; }
    
    public string? Nombre { get; set; }
    
    public ICollection<Interes>? Interes { get; set; }
}