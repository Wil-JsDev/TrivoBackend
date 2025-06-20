using Trivo.Dominio.Enum;

namespace Trivo.Dominio.Modelos;

public sealed class Chat : EntidadBase
{
    public Guid? ChatId { get; set; }
    
    public TipoChat? TipoChat { get; set; }
    
    public string? Nombre { get; set; }
    
    public bool? Activo { get; set; }
    
    public Mensaje? Mensaje { get; set; }
    
    public ICollection<ChatUsuario>? ChatUsuarios { get; set; } 
}