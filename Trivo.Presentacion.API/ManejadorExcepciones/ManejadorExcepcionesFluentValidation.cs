using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace Trivo.Presentacion.API.ManejadorExcepciones;
public class ManejadorExcepcionesGlobal : IExceptionHandler
{
    private readonly ILogger<ManejadorExcepcionesGlobal> _logger;

    public ManejadorExcepcionesGlobal(ILogger<ManejadorExcepcionesGlobal> logger)
    {
        _logger = logger;
    }

    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext,
        Exception exception,
        CancellationToken cancellationToken
    )
    {
        var detallesProblema = new ProblemDetails
        {
            Instance = httpContext.Request.Path
        };

        if (exception is FluentValidation.ValidationException excepcionValidacion)
        {
            detallesProblema.Title = "Ocurrieron uno o más errores de validación.";
            detallesProblema.Type = "https://tools.ietf.org/html/rfc7231#section-6.5.1";
            httpContext.Response.StatusCode = StatusCodes.Status400BadRequest;

            List<string> erroresValidacion = new();

            foreach (var error in excepcionValidacion.Errors)
            {
                erroresValidacion.Add(error.ErrorMessage);
            }

            detallesProblema.Extensions.Add("Errores", erroresValidacion);
        }
        else
        {
            detallesProblema.Title = exception.Message;
        }

        _logger.LogError("Error procesado: {Titulo}", detallesProblema.Title);

        detallesProblema.Status = httpContext.Response.StatusCode;

        await httpContext.Response.WriteAsJsonAsync(detallesProblema, cancellationToken)
            .ConfigureAwait(false);

        return true;
    }
}
