using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace Trivo.Presentacion.API.ManejadorExcepciones;

public class ManejadorDeExcepcionesGlobales : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext, 
        Exception exception, 
        CancellationToken cancellationToken)
    {
        var detallesProblema = new ProblemDetails
        {
            Title = "Error del servidor",
            Status = StatusCodes.Status500InternalServerError,
            Instance = httpContext.Request.Path,
            Detail = exception.Message
        };

        // logger.LogError(exception, "Error general capturado");

        httpContext.Response.StatusCode = detallesProblema.Status.Value;
        httpContext.Response.ContentType = "application/json";

        await httpContext.Response.WriteAsJsonAsync(detallesProblema, cancellationToken);

        return true;
    }
}