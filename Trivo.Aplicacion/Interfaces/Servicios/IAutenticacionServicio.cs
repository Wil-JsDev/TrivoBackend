using Trivo.Aplicacion.DTOs.JWT;
using Trivo.Dominio.Modelos;

namespace Trivo.Aplicacion.Interfaces.Servicios;

public interface IAutenticacionServicio
{
    string GenerarToken(Usuario usuario);
    TokenRefrescadoDTO GenerarTokenRefrescado();
}