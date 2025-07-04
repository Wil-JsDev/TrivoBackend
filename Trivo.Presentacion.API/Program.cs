using Serilog;
using Trivo.Aplicacion;
using Trivo.Infraestructura.Compartido;
using Trivo.Infraestructura.Persistencia;
using Trivo.Presentacion.API.ServiciosDeExtensiones;

try 
{ 
    Log.Information("Iniciando servidor");

    var builder = WebApplication.CreateBuilder(args);

    builder.Host.UseSerilog((context, loggerConfiguration) =>
    {
        loggerConfiguration.ReadFrom.Configuration(context.Configuration);
    });

    // Add services to the container.

    builder.Services.AddControllers();
    // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();
    builder.Services.AgregarExcepciones();

    builder.Services.AgregarPesistencia(builder.Configuration);
    builder.Services.AgregarCapaAplicacion();
    builder.Services.AgregarCapaCompartida(builder.Configuration);
    builder.Services.AgregarExcepciones();
    builder.Services.AgregarVersionado();    
    

    var app = builder.Build();
    
    app.UseExceptionHandler(_ => { });
    
    app.UseSerilogRequestLogging();
    
    // Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    app.UseHttpsRedirection();

    app.UseAuthentication();
    app.UseAuthorization();

    app.MapControllers();

    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex,"Ha ocurrido un error");
}
finally
{
    Log.CloseAndFlush();
}