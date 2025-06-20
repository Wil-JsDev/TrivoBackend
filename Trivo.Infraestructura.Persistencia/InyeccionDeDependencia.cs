using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Trivo.Infraestructura.Persistencia.Contexto;

namespace Trivo.Infraestructura.Persistencia;

public static class InyeccionDeDependencia
{
    public static void AgregarPesistencia(
        this IServiceCollection servicio, 
        IConfiguration conguracion)
    {

        #region DbContexto

            servicio.AddDbContext<TrivoContexto>(postgres =>
            {
                postgres.UseNpgsql(conguracion.GetConnectionString("TrivoBackend"), b =>
                {
                    b.MigrationsAssembly("Trivo.Infraestructura.Persistencia");
                });
            });

        #endregion
        
    }
}