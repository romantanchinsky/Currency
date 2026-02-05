using System.Security.Authentication;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using User.Application.Exceptions;

namespace User.Api.Exceptions;

internal sealed class GloblaExceptionHandler : IExceptionHandler
{
    IProblemDetailsService _problemDetailsService;
    public GloblaExceptionHandler(IProblemDetailsService problemDetailsService)
    {
        _problemDetailsService = problemDetailsService;
    }

    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext,
        Exception exception,
        CancellationToken cancellationToken)
    {
        var (statusCode, detail) = exception switch
        {
            InvalidCredentialsException ex =>
                (StatusCodes.Status401Unauthorized, ex.Message),

            UserAlreadyExistsException ex =>
                (StatusCodes.Status409Conflict, ex.Message),

            _ =>
                (StatusCodes.Status500InternalServerError, "Internal server error")
        };

        httpContext.Response.StatusCode = statusCode;

        return await _problemDetailsService.TryWriteAsync(new ProblemDetailsContext
        {
            HttpContext = httpContext,
            Exception = exception,
            ProblemDetails = new ProblemDetails
            {
                Type = exception.GetType().Name,
                Title = "An error occured",
                Detail = detail
            }
        });
    }
}