namespace Trivo.Aplicacion.DTOs.JWT;

public class TokenRefrescadoDTO
{ 
    public string? UsuarioId { get; set; } 
    public string? Token { get; set; } 
    public DateTime Expira { get; set; } 
    public bool EstaExpirado => DateTime.UtcNow > Expira; 
    public DateTime Creado { get; set; } public DateTime? Revocado { get; set; } 
    public string? ReemplazadoPorToken { get; set; } 
    public bool EstaActivo => Revocado == null && !EstaExpirado;
    

}