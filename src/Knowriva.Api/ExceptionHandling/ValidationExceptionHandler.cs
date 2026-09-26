using FluentValidation;
using Microsoft.AspNetCore.Diagnostics;

namespace Knowriva.Api.ExceptionHandling;

public sealed class ValidationExceptionHandler : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(
        HttpContext context,
        Exception exception,
        CancellationToken cancellationToken)
    {
        if (exception is not ValidationException validationException)
        {
            return false;
        }

        var errors = validationException.Errors
            .GroupBy(error => error.PropertyName)
            .ToDictionary(
                group => group.Key,
                group => group.Select(error => error.ErrorMessage).ToArray());

        await Results.ValidationProblem(errors).ExecuteAsync(context);
        return true;
    }
}
