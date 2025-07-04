using Asp.Versioning;
using Trivo.Presentacion.API.Middlewares;

namespace Trivo.Presentacion.API.ServiciosDeExtensiones;

public static class ServicioExtensiones
{

    public static void UseManejadorErroresPersonalizado(this IApplicationBuilder construir)
    {
        construir.UseMiddleware<MiddlewareCapturaExcepciones>();
    }
    
    public static void AgregarVersionado(this IServiceCollection services)
    {
        services.AddApiVersioning(options =>
        {
            options.DefaultApiVersion = new ApiVersion(1, 0);
            options.AssumeDefaultVersionWhenUnspecified = true; //When no versions are sent, this assumes the default version which is V1
            options.ReportApiVersions = true;
        });
    }
    
}