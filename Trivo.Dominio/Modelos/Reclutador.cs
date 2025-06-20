namespace Trivo.Dominio.Modelos;

public sealed class Reclutador : EntidadBase
{
    public string? NombreEmpresa { get; set; }
    
    public Guid? UsuarioId { get; set; }
    
    public Usuario? Usuario { get; set; }
    
    public ICollection<Emparejamiento>?  Emparejamientos { get; set; }
}