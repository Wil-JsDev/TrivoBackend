using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Trivo.Aplicacion.Interfaces.Repositorio;
using Trivo.Infraestructura.Persistencia.Contexto;
using Trivo.Infraestructura.Persistencia.Repositorio;

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

        #region ID

        servicio.AddTransient(typeof(IRepositorioGenerico<>), typeof(RepositorioGenerico<>));

        #endregion
        
    }
}