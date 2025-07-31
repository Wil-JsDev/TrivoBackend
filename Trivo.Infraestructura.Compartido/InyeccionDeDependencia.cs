using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using Trivo.Aplicacion.DTOs.JWT;
using Trivo.Aplicacion.Interfaces.Servicios;
using Trivo.Aplicacion.Interfaces.Servicios.IA;
using Trivo.Aplicacion.Interfaces.Servicios.SignaIR;
using Trivo.Dominio.Configuraciones;
using Trivo.Infraestructura.Compartido.Servicios;
using Trivo.Infraestructura.Compartido.SignalR;

namespace Trivo.Infraestructura.Compartido;

public static class InyeccionDeDependencia
{
    public static void AgregarCapaCompartida(this IServiceCollection servicio, IConfiguration configuraciones)
    {
        #region Configuraciones

            servicio.Configure<EmailConfiguraciones>(configuraciones.GetSection("EmailConfiguraciones"));        
            servicio.Configure<CloudinaryConfiguraciones>(configuraciones.GetSection("CloudinaryConfiguraciones"));
            servicio.Configure<JWTConfiguraciones>(configuraciones.GetSection("JWTConfiguraciones"));
            servicio.Configure<OllamaOpciones>(configuraciones.GetSection("OllamaOpciones"));

            servicio.AddHttpClient<IOllamaServicio, OllamaServicio>((client) =>
            {
                client.BaseAddress = new Uri(configuraciones["OllamaOpciones:BaseUrl"] ?? string.Empty);
            });
            
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
                    ValidIssuer = configuraciones["JWTConfiguraciones:Emisor"],
                    ValidAudience = configuraciones["JWTConfiguraciones:Audiencia"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuraciones["JWTConfiguraciones:Clave"] ?? string.Empty))
                };
                options.Events = new JwtBearerEvents()
                {
                    OnAuthenticationFailed = context =>
                    {
                        if (context.Exception is SecurityTokenExpiredException)
                        {
                            context.Response.StatusCode = 401;
                            context.Response.ContentType = "application/json";
                            var resultado = JsonConvert.SerializeObject(new JWTRespuesta(true, "El token ha expirado"));
                            return context.Response.WriteAsync(resultado);
                        }

                        context.Response.StatusCode = 401;
                        context.Response.ContentType = "application/json";
                        var resultadoGeneral = JsonConvert.SerializeObject(new JWTRespuesta(true, "Token inválido o error de autenticación"));
                        return context.Response.WriteAsync(resultadoGeneral);
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
                    },
                    OnMessageReceived = context =>
                    {
                        var accessToken = context.Request.Query["access_token"];
                        var path = context.HttpContext.Request.Path;
    
                        if (!string.IsNullOrEmpty(accessToken) && path.StartsWithSegments("/hubs"))
                        {
                            context.Token = accessToken;
                        }
                        return Task.CompletedTask;
                    }
                };

            });
            
            #endregion
        
        #region Servicios
            
            servicio.AddScoped<IEmailServicio, EmailServicio>();
            servicio.AddScoped<ICloudinaryServicio, CloudinaryServicio>();
            servicio.AddScoped<IAutenticacionServicio, AutenticacionServicio>();
            servicio.AddScoped<IOllamaServicio, OllamaServicio>();
            
        #endregion

        #region SignalR

            servicio.AddSignalR();
            
            servicio.AddSingleton<INotificadorIA, NotificadorIA>();
            servicio.AddSingleton<IUserIdProvider, CustomUserIdProvider>();
            servicio.AddTransient<INotificadorTiempoReal, NotificadorTiempoReal>();
            servicio.AddScoped<INotificadorDeEmparejamiento, NotificadorDeEmparejamiento>();

        #endregion
    }
    
}