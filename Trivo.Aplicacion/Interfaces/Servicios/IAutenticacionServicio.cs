using Trivo.Aplicacion.DTOs.JWT;
using Trivo.Aplicacion.Utilidades;
using Trivo.Dominio.Modelos;

namespace Trivo.Aplicacion.Interfaces.Servicios;

public interface IAutenticacionServicio
{
    Task<string> GenerarToken(Usuario usuario,CancellationToken cancellationToken);

    string GenerarTokenAdministrador(Administrador admin);
    
    Task<ResultadoT<TokenRespuestaDto>> RefrescarTokenAsync(string refreshToken, CancellationToken cancellationToken);

    string GenerarRefreshToken(Usuario usuario);
    
    string GenerarRefreshTokenAdministrador(Administrador admin);
}