using Trivo.Presentacion.API.ManejadorExcepciones;

namespace Trivo.Presentacion.API.ServiciosDeExtensiones;

public static class ServicioExtensiones
{
    public static void AgregarExcepciones(this IServiceCollection servicio)
    {
        servicio.AddExceptionHandler<ManejadorExcepcionesGlobal>();
    }
    
}