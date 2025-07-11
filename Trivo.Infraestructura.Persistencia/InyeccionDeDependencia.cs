using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Trivo.Aplicacion.Interfaces.Base;
using Trivo.Aplicacion.Interfaces.Repositorio;
using Trivo.Aplicacion.Interfaces.Repositorio.Cuenta;
using Trivo.Aplicacion.Interfaces.Servicios;
using Trivo.Infraestructura.Persistencia.Base;
using Trivo.Infraestructura.Persistencia.Contexto;
using Trivo.Infraestructura.Persistencia.Repositorio;
using Trivo.Infraestructura.Persistencia.Repositorio.Cuenta;
using Trivo.Infraestructura.Persistencia.Servicios;

namespace Trivo.Infraestructura.Persistencia;

public static class InyeccionDeDependencia
{
    public static void AgregarPesistencia(
        this IServiceCollection servicio, 
        IConfiguration conguracion)
    {

        #region Redis
            string conexionString = conguracion.GetConnectionString("Redis")!;
            servicio.AddStackExchangeRedisCache(opciones =>
            {
                opciones.Configuration = conexionString;
            });
        #endregion
        
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
            servicio.AddTransient(typeof(IValidacion<>), typeof(Validacion<>));
            servicio.AddTransient<IRepositorioAdministrador, RepositorioAdministrador>();
            servicio.AddTransient<IRepositorioExperto, RepositorioExperto>();
            servicio.AddTransient<IRepositorioReclutador, RepositorioReclutador>();
            servicio.AddTransient<IRepositorioCodigo,  RepositorioCodigo>();
            servicio.AddTransient<IRepositorioUsuario, RepositorioUsuario>();
            servicio.AddTransient<IRepositorioHabilidad, RepositorioHabilidad>();
            servicio.AddTransient<IRepositorioUsuarioHabilidad, RepositorioUsuarioHabilidad>();
            servicio.AddTransient<IRepositorioInteres, RepositorioInteres>();
            servicio.AddTransient<IRepositorioUsuarioInteres, RepositorioUsuarioInteres>();
            servicio.AddTransient<IRepositorioCategoriaInteres,  RepositorioCategoriaInteres>();
            servicio.AddTransient<IRepositorioEmparejamiento, RepositorioEmparejamiento>();
            servicio.AddScoped<IRolUsuarioServicio, RolUsuarioServicio>();
            servicio.AddScoped<IObtenerExpertoIdServicio, ObtenerExpertoIdServicio>();
            servicio.AddScoped<IObtenerReclutadorIdServicio, ObtenerReclutadorIdServicio>();
            servicio.AddScoped<IRepositorioChat, RepositorioChat>();    
        
        #endregion

    }
}