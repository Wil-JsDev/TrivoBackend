namespace Trivo.Dominio.Modelos;

public sealed class Experto : EntidadBase
{
    public Guid? UsuarioId { get; set; }
    
    public bool? DisponibleParaProyectos { get; set; }
    
    public bool? Contratado { get; set; }
    
    public Usuario? Usuario { get; set; }
    
    public ICollection<Emparejamiento>?  Emparejamientos { get; set; }
}