using Microsoft.EntityFrameworkCore;
using Serilog;
using Trivo.Aplicacion;
using Trivo.Infraestructura.Compartido;
using Trivo.Infraestructura.Compartido.SignalR.Hubs;
using Trivo.Infraestructura.Persistencia;
using Trivo.Infraestructura.Persistencia.Contexto;
using Trivo.Presentacion.API.ServiciosDeExtensiones;

try
{
    Log.Information("Iniciando servidor");

    var builder = WebApplication.CreateBuilder(args);

    builder.Host.UseSerilog((context, loggerConfiguration) =>
    {
        loggerConfiguration.ReadFrom.Configuration(context.Configuration);
    });

    builder.Configuration
        .SetBasePath(Directory.GetCurrentDirectory())
        .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
        .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true, reloadOnChange: true)
        .AddEnvironmentVariables();

    // Add services to the container.

    builder.Services.AddControllers();
    // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();

    builder.Services.AgregarPesistencia(builder.Configuration);
    builder.Services.AgregarCapaAplicacion();
    builder.Services.AgregarCapaCompartida(builder.Configuration);
    builder.Services.AgregarVersionado();

    builder.Services.AddCors(options =>
    {
        options.AddPolicy("AllowFrontendDev", policy =>
        {
            policy.WithOrigins("http://127.0.0.1:5500", "http://localhost:3000","http://localhost:3008")
                .AllowAnyHeader()
                .AllowAnyMethod()
                .AllowCredentials();
        });
    });

    var app = builder.Build();

    using (var scope = app.Services.CreateScope())
    {
        var db = scope.ServiceProvider.GetRequiredService<TrivoContexto>();
        db.Database.Migrate(); // Aplica las migraciones automáticamente
    }

    app.UseExceptionHandler(_ => { });

    app.UseCors("AllowFrontendDev");

    app.UseRouting();

    app.UseWebSockets();

    app.UseSerilogRequestLogging();

    // Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment() || app.Environment.IsProduction())
    {
        app.UseSwagger();
        app.UseSwaggerUI(c =>
        {
            c.SwaggerEndpoint("/swagger/v1/swagger.json", "API v1");
            c.RoutePrefix = "swagger";
        });
    }

    app.UseHttpsRedirection();

    app.UseManejadorErroresPersonalizado();

    app.UseAuthentication();
    app.UseAuthorization();

    app.MapControllers();

    app.MapHub<ChatHub>("/hubs/chat");
    app.MapHub<RecomendacionUsuariosHub>("/hubs/recomendaciones");
    app.MapHub<EmparejamientoHub>("hubs/emparejamientos");
    app.MapHub<NotificacionHub>("/hubs/notificaciones");
    
    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Ha ocurrido un error");
}
finally
{
    Log.CloseAndFlush();
}