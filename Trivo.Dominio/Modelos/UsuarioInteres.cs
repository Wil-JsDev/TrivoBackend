namespace Trivo.Dominio.Modelos;

public sealed class UsuarioInteres
{
    public Guid? UsuarioId { get; set; }

    public Guid? InteresId { get; set; }

    public Usuario? Usuario { get; set; }
    
    public Interes? Interes { get; set; }
}
