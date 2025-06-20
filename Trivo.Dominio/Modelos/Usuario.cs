using Trivo.Dominio.Enum;

namespace Trivo.Dominio.Modelos;

public sealed class Usuario : EntidadBase
{
    public string? Nombre { get; set; }
    
    public string? Apellido { get; set; }
    
    public string? Biografia { get; set; }
    
    public bool? CuentaConfirmada { get; set; }
    
    public string? Email { get; set; }
    
    public string? ContrasenaHash { get; set; }

    public string? NombreUsuario { get; set; }
    
    public string? Ubicacion { get; set; }
    
    public string? FotoPerfil { get; set; }
    
    public string Linkedin  { get; set; }

    public EstadoUsuario? EstadoUsuario { get; set; }
    
    // Relaciones
    public ICollection<Codigo>? Codigos { get; set; }
    
    public ICollection<UsuarioInteres>? UsuarioInteres { get; set; }
    
    public ICollection<Interes> Interes { get; set; } = new List<Interes>();
    
    public ICollection<UsuarioHabilidad>? UsuarioHabilidades { get; set; } 
    
    public ICollection<ChatUsuario>? ChatUsuarios { get; set; } 
    
    public ICollection<Mensaje>? Mensajes { get; set; }
    
    public ICollection<Notificacion>? Notificaciones { get; set; }
    
    public ICollection<Experto>? Expertos { get; set; }
    
    public ICollection<Reclutador>? Reclutadores { get; set; }
 
    public ICollection<Reporte>? Reportes { get; set; }
}
