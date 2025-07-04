using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace Trivo.Presentacion.API.ManejadorExcepciones;
public class ManejadorExcepcionesFluentValidation(ILogger<ManejadorExcepcionesFluentValidation> logger)
    : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext,
        Exception exception,
        CancellationToken cancellationToken
    )
    {
        if (exception is not FluentValidation.ValidationException excepcionValidacion)
            return false; // No es esta excepción, que siga otro handler

        var detallesProblema = new ProblemDetails
        {
            Title = "Ocurrieron uno o más errores de validación.",
            Status = StatusCodes.Status400BadRequest,
            Instance = httpContext.Request.Path,
            Detail = excepcionValidacion.Message
        };

        List<string> erroresValidacion = excepcionValidacion.Errors
            .Select(e => e.ErrorMessage)
            .ToList();

        detallesProblema.Extensions.Add("errors", erroresValidacion);

        logger.LogError("Error de validación: {Title}", detallesProblema.Title);

        httpContext.Response.StatusCode = detallesProblema.Status.Value;
        httpContext.Response.ContentType = "application/json";

        await httpContext.Response.WriteAsJsonAsync(detallesProblema, cancellationToken);

        return true; // Excepción manejada
    }
}
