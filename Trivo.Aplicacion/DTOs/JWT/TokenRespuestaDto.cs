namespace Trivo.Aplicacion.DTOs.JWT;

public class TokenRespuestaDto
{
    public string? TokenAcceso { get; set; }
    
    public string? TokenRefresco { get; set; }
    
    public DateTime Expira { get; set; }
}