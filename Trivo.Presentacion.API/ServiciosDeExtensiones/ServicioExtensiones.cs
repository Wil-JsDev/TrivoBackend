using Asp.Versioning;
using Trivo.Presentacion.API.ManejadorExcepciones;

namespace Trivo.Presentacion.API.ServiciosDeExtensiones;

public static class ServicioExtensiones
{
    public static void AgregarExcepciones(this IServiceCollection servicio)
    {
        servicio.AddExceptionHandler<ManejadorDeExcepcionesGlobales>();
        servicio.AddExceptionHandler<ManejadorExcepcionesFluentValidation>();
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