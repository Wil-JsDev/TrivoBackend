using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using Trivo.Aplicacion.DTOs.JWT;
using Trivo.Aplicacion.Interfaces.Servicios;
using Trivo.Dominio.Configuraciones;
using Trivo.Infraestructura.Compartido.Servicios;

namespace Trivo.Infraestructura.Compartido;

public static class InyeccionDeDependencia
{
    public static void AgregarCapaCompartida(this IServiceCollection servicio, IConfiguration configuraciones)
    {
        #region Configuraciones

            servicio.Configure<EmailConfiguraciones>(configuraciones.GetSection("EmailConfiguraciones"));        
            servicio.Configure<CloudinaryConfiguraciones>(configuraciones.GetSection("CloudinaryConfiguraciones"));        
            
        #endregion
        
        #region JWT

            servicio.Configure<JWTConfiguraciones>(configuraciones.GetSection("JWTConfiguraciones"));
            servicio.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;

            }).AddJwtBearer(options =>
            {
                options.RequireHttpsMetadata = false;
                options.SaveToken = false;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero,
                    ValidIssuer = configuraciones["JWTConfiguraciones:Issuer"],
                    ValidAudience = configuraciones["JWTConfiguraciones:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuraciones["JWTConfiguraciones:Key"]))
                };
                options.Events = new JwtBearerEvents()
                {
                    OnAuthenticationFailed = c =>
                    {
                        c.NoResult();
                        c.Response.StatusCode = 500;
                        c.Response.ContentType = "text/plain";
                        return c.Response.WriteAsync(c.Exception.ToString());
                    },

                    OnChallenge = c =>
                    {
                        c.HandleResponse();
                        c.Response.StatusCode = 401;
                        c.Response.ContentType = "application/json";
                        var result = JsonConvert.SerializeObject(new JWTRespuesta(true, "Ocurrió un error inesperado en la autenticación"));
                        return c.Response.WriteAsync(result);
                    },
                    OnForbidden = c =>
                    {
                        c.Response.StatusCode = 403;
                        c.Response.ContentType = "application/json";
                        var result = JsonConvert.SerializeObject(new JWTRespuesta(true,
                            "No estás autorizado para acceder a este contenido"));

                        return c.Response.WriteAsync(result);
                    }
                };

            });
            
            #endregion
        
        #region Servicios
            
            servicio.AddScoped<IEmailServicio, EmailServicio>();
            servicio.AddScoped<ICloudinaryServicio, CloudinaryServicio>();
            
        #endregion        
    }
    
}