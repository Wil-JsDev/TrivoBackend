namespace Trivo.Dominio.Configuraciones;

public sealed class EmailConfiguraciones
{
    public string? EmailFrom { get; set;}
    
    public string? SmtpHost { get; set;}
    
    public int SmtpPort { get; set;}
    
    public string? SmtpUser { get; set;}
    
    public string? SmtpPass { get; set;}
    
    public string? DisplayName { get; set;}
}