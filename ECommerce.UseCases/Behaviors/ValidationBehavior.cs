using ECommerce.Domain.Common;
using ECommerce.Domain.Common.Errors;
using FluentValidation;
using MediatR;

namespace ECommerce.UseCases.Behaviors;

public class ValidationBehavior<TRequest, TResponse>(IEnumerable<IValidator<TRequest>> validators)
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : notnull
{
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        if (!validators.Any())
        {
            return await next(cancellationToken);
        }

        var context = new ValidationContext<TRequest>(request);
        var results = await Task.WhenAll(validators.Select(v => v.ValidateAsync(context, cancellationToken)));

        var failures = results.SelectMany(r => r.Errors).Where(f => f is not null).ToList();
        
        if(failures.Count != 0)
        {
            // create validation error   
            var validationError = Error.Validation(
                "Validation.Failed",
                failures.First().ErrorMessage);

            // Get return type Result or Result<>
            var responseType = typeof(TResponse);

            // Generic Result<> Handle
            if (responseType.IsGenericType && responseType.GetGenericTypeDefinition() == typeof(Result<>))
            {
                var resultType = responseType.GetGenericArguments()[0];
                var failureMethod = typeof(Result<>)
                    .MakeGenericType(resultType)
                    .GetMethod(nameof(Result<object>.Failure));

                return (TResponse)failureMethod!.Invoke(null, [validationError])!;
            }
            // Non-Generic Result Handle
            else if (responseType == typeof(Result))
            {
                return (TResponse)(object)Result.Failure(validationError);
            }

            throw new ValidationException(failures);
        }

        return await next(cancellationToken);
    }
}
