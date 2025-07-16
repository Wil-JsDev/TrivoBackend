using Trivo.Dominio.Enum;

namespace Trivo.Dominio.Modelos;

public sealed class UsuarioHabilidad
{
    public Guid? UsuarioId { get; set; }
    
    public Guid? HabilidadId { get; set; }
    
    public Usuario? Usuario { get; set; }
    
    public Habilidad? Habilidad { get; set; }
}
