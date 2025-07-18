namespace Trivo.Dominio.Modelos;

public sealed class ChatUsuario
{
    public Guid? ChatId { get; set; }
    
    public Chat? Chat { get; set; }
    public string NombreChat { get; set; }
    
    public Guid? UsuarioId { get; set; }
    
    public Usuario? Usuario { get; set; }
    
    public DateTime? FechaIngreso { get; set; } = DateTime.UtcNow;
    
    public DateTime? FechaSalida { get; set; }
}