using Trivo.Dominio.Enum;

namespace Trivo.Dominio.Modelos;

public sealed class Reporte
{
    public Guid? ReporteId { get; set; }
    
    public Guid? ReportadoPor { get; set; }
    
    public Guid? MensajeId { get; set; }
    
    public string? EstadoReporte { get; set; }
    
    public string? Nota { get; set; }
    
    public Mensaje? Mensaje { get; set; }
    
    public Usuario? Usuario { get; set; }
}