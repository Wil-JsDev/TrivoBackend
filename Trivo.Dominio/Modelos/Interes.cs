namespace Trivo.Dominio.Modelos;

public sealed class  Interes : EntidadBase
{
    public string? Nombre { get; set; }
    
    public Guid? CategoriaId { get; set; }
    
    public CategoriaInteres? Categoria { get; set; }
    
    public Guid? CreadoPor { get; set; }
    
    public Usuario? Usuario { get; set; }
    
    public ICollection<CategoriaInteres>? Categorias { get; set; }
    
    public ICollection<UsuarioInteres> UsuarioInteres { get; set; } = new List<UsuarioInteres>();
}