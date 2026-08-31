using CloudStorage.Exceptions;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace CloudStorage.Middlewares;

public sealed class NotFoundExceptionHandler(
    IProblemDetailsService problemDetailsService)
    : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext,
        Exception exception,
        CancellationToken cancellationToken)
    {
        if (exception is not NotFoundException notFound)
        {
            return false;
        }

        httpContext.Response.StatusCode = StatusCodes.Status404NotFound;

        await problemDetailsService.WriteAsync(new ProblemDetailsContext
        {
            HttpContext = httpContext,
            Exception = notFound,
            ProblemDetails = new ProblemDetails
            {
                Status = StatusCodes.Status404NotFound,
                Title = "Resource Not Found",
                Detail = notFound.Message
            }
        });

        return true;
    }
}