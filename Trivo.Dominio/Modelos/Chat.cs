using Trivo.Dominio.Enum;

namespace Trivo.Dominio.Modelos;

public sealed class Chat : EntidadBase
{
    public string? TipoChat { get; set; }
    
    public bool? Activo { get; set; }
    
    public ICollection<Mensaje> Mensajes { get; set; } = new List<Mensaje>();
    
    public ICollection<ChatUsuario>? ChatUsuarios { get; set; } 
}