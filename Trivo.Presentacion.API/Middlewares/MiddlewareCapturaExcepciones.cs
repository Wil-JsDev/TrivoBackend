using FluentValidation;
using Microsoft.AspNetCore.Mvc;

namespace Trivo.Presentacion.API.Middlewares;

public class MiddlewareCapturaExcepciones(RequestDelegate siguiente, ILogger<MiddlewareCapturaExcepciones> logger)
{
    public async Task InvokeAsync(HttpContext context)
    {
        var rastreoId = Guid.NewGuid().ToString();
        context.Response.Headers["rastreo-id"] = rastreoId;
        try
        {
            await siguiente(context);
        }
        catch (ValidationException ex)
        {
            logger.LogError("Error de validación capturado: {Mensaje} RastreoId:{RastreoId} Ruta:{Ruta} ", ex.Message, rastreoId, context.Request.Path);
        
            var problemDetails = new ProblemDetails
            {
                Status = StatusCodes.Status400BadRequest,
                Title = "Errores de validaciones",
                Detail = "Tienes uno o mas errores de validaciones",
                Type = "ValidationFailure",
                Instance = context.Request.Path
            };
        
            problemDetails.Extensions["errors"] = ex.Errors
                .GroupBy(e => e.PropertyName)
                .ToDictionary(
                    g => g.Key,
                    g => g.Select(e => e.ErrorMessage).ToArray()
                );
        
            context.Response.StatusCode = StatusCodes.Status400BadRequest;
            context.Response.ContentType = "application/json";
            await context.Response.WriteAsJsonAsync(problemDetails);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Excepción general no controlada");
        
            var problemDetails = new ProblemDetails
            {
                Title = "Server error",
                Status = StatusCodes.Status500InternalServerError,
                Detail = ex.Message,
                Instance = context.Request.Path
            };
        
            context.Response.StatusCode = StatusCodes.Status500InternalServerError;
            context.Response.ContentType = "application/json";
            await context.Response.WriteAsJsonAsync(problemDetails);
        }
    }
}
