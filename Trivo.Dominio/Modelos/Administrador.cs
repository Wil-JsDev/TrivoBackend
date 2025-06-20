namespace Trivo.Dominio.Modelos;

public sealed class Administrador : EntidadBase
{
    public string? Nombre { get; set; }
    
    public string? Apellido { get; set; }
    
    public string? Biografia { get; set; }
    
    public string? Email { get; set; }
    
    public string? ContrasenaHash { get; set; }

    public string? NombreUsuario { get; set; }
    
    public string? FotoPerfil { get; set; }
    
    public bool? Activo { get; set; } = true;
    
    public string Linkedin  { get; set; }
}