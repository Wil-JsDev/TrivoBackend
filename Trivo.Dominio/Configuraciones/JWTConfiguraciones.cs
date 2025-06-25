namespace Trivo.Dominio.Configuraciones;

public class JWTConfiguraciones
{
    public string? Clave { get; set; }
    public string? Emisor { get; set; }       
    public string? Audiencia { get; set; }    
    public int DuracionEnMinutos { get; set; }
}