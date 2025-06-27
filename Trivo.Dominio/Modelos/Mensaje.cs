using Trivo.Dominio.Enum;

namespace Trivo.Dominio.Modelos;

public sealed class Mensaje
{
    public Guid? MensajeId { get; set; }
    
    public Guid? ChatId { get; set; }
    
    public Guid? EmisorId { get; set; }
    
    public string? Contenido { get; set; }
    
    public string? Estado { get; set; }
    
    public DateTime? FechaEnvio { get; set; }
    
    public DateTime? FechaRegistro { get; set; } = DateTime.UtcNow;
    
    public Usuario? Usuario { get; set; }
    
    public Chat? Chat { get; set; }
    
    public ICollection<Reporte>? Reportes { get; set; }
    
    public ICollection<Chat>? Chats { get; set; }
}