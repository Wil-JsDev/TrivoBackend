using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Trivo.Aplicacion.DTOs.JWT;
using Trivo.Aplicacion.Interfaces.Repositorio.Cuenta;
using Trivo.Aplicacion.Interfaces.Servicios;
using Trivo.Aplicacion.Utilidades;
using Trivo.Dominio.Configuraciones;
using Trivo.Dominio.Enum;
using Trivo.Dominio.Modelos;

namespace Trivo.Infraestructura.Compartido.Servicios;

public class AutenticacionServicio(
    IOptions<JWTConfiguraciones> configuraciones, 
    IRolUsuarioServicio rolUsuarioServicio,
    IRepositorioUsuario repositorioUsuarios,
    IObtenerExpertoIdServicio obtenerExpertoIdServicio,
    IObtenerReclutadorIdServicio obtenerReclutadorIdServicio
    ) : IAutenticacionServicio
{
    private readonly JWTConfiguraciones _configuraciones = configuraciones.Value;

    public async Task<string> GenerarToken(Usuario usuario, CancellationToken cancellationToken)
    {
        var claims = new List<Claim>
        {
            new Claim(JwtRegisteredClaimNames.Sub, usuario.Id.ToString()!),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim(JwtRegisteredClaimNames.Email, usuario.Email!),
            new Claim("nombreUsuario", usuario.NombreUsuario!),
            new Claim("tipo","access")
        };

        //Agregar
        var roles = await rolUsuarioServicio.ObtenerRolesAsync(usuario.Id ?? Guid.Empty, cancellationToken);
        claims.AddRange(roles.Select(rol => new Claim("roles", rol.ToString())));

        // Obtener experto id si aplica
        var experto = await obtenerExpertoIdServicio.ObtenerExpertoIdAsync(usuario.Id ?? Guid.Empty, cancellationToken);
        if (experto.HasValue)
        {
            claims.Add(new Claim("expertoId", experto.Value.ToString()));
        }
        
        // Obtener reclutador id si aplica
        var reclutador = await obtenerReclutadorIdServicio.ObtenerReclutadorIdAsync(usuario.Id ?? Guid.Empty, cancellationToken);
        if (reclutador.HasValue)
        {
            claims.Add(new Claim("reclutadorId", reclutador.Value.ToString()));
        }

        var clave = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuraciones.Clave!));
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
    
    public async Task<ResultadoT<TokenRespuestaDto>> RefrescarTokenAsync(string refreshToken, CancellationToken cancellationToken)
    {
        var manejador = new JwtSecurityTokenHandler();

        var tokenValidado = manejador.ValidateToken(refreshToken, new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidIssuer = _configuraciones.Emisor,
            ValidAudience = _configuraciones.Audiencia,
            ValidateLifetime = false,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuraciones.Clave!)),
            ClockSkew = TimeSpan.Zero
        }, out SecurityToken validatedToken);

        var jwt = (JwtSecurityToken)validatedToken;

        if (jwt.Claims.FirstOrDefault(c => c.Type == "tipo")?.Value != "refresh")
            return ResultadoT<TokenRespuestaDto>.Fallo(Error.NoAutorizado("Token.Invalido", "El token no es de tipo refresh."));
        
        var usuarioId = jwt.Subject;
        var usuario = await repositorioUsuarios.ObtenerByIdAsync(Guid.Parse(usuarioId), cancellationToken);

        if (usuario is null)
            return ResultadoT<TokenRespuestaDto>.Fallo(Error.NoEncontrado("404", "Usuario no encontrado."));
        
        var nuevoTokenAcceso = await GenerarToken(usuario, cancellationToken);
        var nuevoRefreshToken = GenerarRefreshToken(usuario);
        
        return ResultadoT<TokenRespuestaDto>.Exito(new TokenRespuestaDto
        {
            TokenAcceso = nuevoTokenAcceso,
            TokenRefresco = nuevoRefreshToken,
        });
    }
    public string GenerarRefreshToken(Usuario usuario)
    {
        var claims = new List<Claim>
        {
            new Claim(JwtRegisteredClaimNames.Sub, usuario.Id.ToString()!),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim("tipo", "refresh")
        };

        var clave = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuraciones.Clave!));
        
        var credenciales = new SigningCredentials(clave, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: _configuraciones.Emisor,
            audience: _configuraciones.Audiencia,
            claims: claims,
            expires: DateTime.UtcNow.AddDays(7), // duración más larga
            signingCredentials: credenciales
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
        
}