using Trivo.Aplicacion.DTOs.JWT;
using Trivo.Dominio.Modelos;

namespace Trivo.Aplicacion.Interfaces.Servicios;

public interface IAutenticacionServicio
{
    Task<string> GenerarToken(Usuario usuario);
    Task<TokenRefrescadoDTO> GenerarTokenRefrescado();
}