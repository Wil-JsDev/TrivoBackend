
namespace Trivo.Dominio.Modelos;

public sealed class Notificacion
{
    public Guid? NotificacionId { get; set; }
    
    public Guid? UsuarioId { get; set; }
    
    public Usuario? Usuario { get; set; }
    
    public string? Tipo { get; set; }
    
    public string? Contenido { get; set; }
    
    public DateTime? FechaCreacion { get; set; } = DateTime.UtcNow;
    
    public bool? Leida { get; set; }
    
    public DateTime? FechaLeida { get; set; }
    
}