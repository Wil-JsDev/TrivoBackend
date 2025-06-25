using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Trivo.Aplicacion.DTOs.JWT;
using Trivo.Aplicacion.Interfaces.Servicios;
using Trivo.Dominio.Configuraciones;
using Trivo.Dominio.Enum;
using Trivo.Dominio.Modelos;

namespace Trivo.Infraestructura.Compartido.Servicios;

public class AutenticacionServicio: IAutenticacionServicio
{
    private JWTConfiguraciones _configuraciones;

    public AutenticacionServicio(IOptions<JWTConfiguraciones> configuraciones)
    {
        _configuraciones = configuraciones.Value;
    }
    
    public async Task<string> GenerarToken(Usuario usuario)
    {
        var claims = new List<Claim>
        {
            new Claim(JwtRegisteredClaimNames.Sub, usuario.Id.ToString()),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim(JwtRegisteredClaimNames.Email, usuario.Email),
            new Claim("nombreUsuario", usuario.NombreUsuario!)
        };
        
        if (usuario.Reclutadores?.Any() is true)
            claims.Add(new Claim("roles", Roles.Administrador.ToString()));

        if (usuario.Expertos?.Any() is true)
            claims.Add(new Claim("roles", Roles.Experto.ToString()));
        
        claims.Add(new Claim("roles", Roles.Administrador.ToString()));
        
        
        var clave = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuraciones.Clave));
        var credenciales = new SigningCredentials(clave, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: _configuraciones.Emisor,
            audience: _configuraciones.Audiencia,
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(_configuraciones.DuracionEnMinutos),
            signingCredentials: credenciales
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    public async Task<TokenRefrescadoDTO> GenerarTokenRefrescado() 
    {
        return new TokenRefrescadoDTO()
        {
            Token = RandomTokenString(),
            Expira = DateTime.UtcNow.AddDays(7),
            Creado = DateTime.UtcNow
        };
    }
    
    private string RandomTokenString()
    {
        using var rng = RandomNumberGenerator.Create();
        var randomBytes = new Byte[40];
        rng.GetBytes(randomBytes);
        return BitConverter.ToString(randomBytes);
    }
}