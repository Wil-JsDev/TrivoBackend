using FluentValidation;
using MediatR;

namespace Trivo.Aplicacion.Comportamiento;

public class ValidacionComportamiento <TRequest, TResponse>(IEnumerable<IValidator<TRequest>> validadores)
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : class
{
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(next);

        if (validadores.Any())
        {
            var context = new ValidationContext<TRequest>(request);
            var validationResults = await Task.WhenAll(
                validadores.Select(v => 
                    v.ValidateAsync(context, cancellationToken))).ConfigureAwait(false);
            var failures = validationResults
                .Where(v => v.Errors.Count > 0)
                .SelectMany(r => r.Errors)
                .ToList();

            if (failures.Count > 0)
                throw new FluentValidation.ValidationException(failures);
        }
        
        return await next().ConfigureAwait(false);
    }
}