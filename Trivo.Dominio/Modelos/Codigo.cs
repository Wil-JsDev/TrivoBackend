namespace Trivo.Dominio.Modelos;

public sealed class Codigo
{
    public Guid? CodigoId { get; set; }
    
    public Guid? UsuarioId { get; set; }
    
    public Usuario? Usuarios { get; set; }
    
    public string? Valor { get; set; }

    public bool? Usado { get; set; } = false;
    
    public DateTime? Expiracion { get; set; }
    
    public DateTime? Creado { get; set; } = DateTime.UtcNow;

    public bool? Revocado { get; set; } = false;
    
    public bool? RefrescarCodigo  { get; set; }
}