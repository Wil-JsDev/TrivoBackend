using System.Reflection;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Trivo.Aplicacion.Comportamiento;
using Trivo.Aplicacion.Interfaces.Servicios;
using Trivo.Aplicacion.Servicios;

namespace Trivo.Aplicacion;

public static class InyeccionDeDependencia
{
    public static void AgregarCapaAplicacion(this IServiceCollection servicio)
    {
        servicio.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
        servicio.AddMediatR(config =>
        {
            config.RegisterServicesFromAssemblies(Assembly.GetExecutingAssembly());
            config.AddOpenBehavior(typeof(ValidacionComportamiento<,>));
        });
        
        servicio.AddProblemDetails();

        #region Servicios

            servicio.AddScoped<ICodigoServicio, CodigoServicio>();
            servicio.AddScoped<IValidarCorreo, ValidarCorreo>();
            servicio.AddScoped<INotificacionServicio, NotificacionServicio>();
            
        #endregion
        
        servicio.AddHttpContextAccessor();
        
    }
}