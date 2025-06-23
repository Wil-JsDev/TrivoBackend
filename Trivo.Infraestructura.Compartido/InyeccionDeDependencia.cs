using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
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

        #endregion
        
        #region Servicios
            
            servicio.AddScoped<IEmailServicio, EmailServicio>();
        
        #endregion        
    }
    
}