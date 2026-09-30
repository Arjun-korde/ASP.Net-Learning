using System;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace server.Exceptions;

public class GlobalExceptionHandler : IExceptionHandler
{
    private readonly ILogger<GlobalExceptionHandler> _logger;

    public GlobalExceptionHandler(
        ILogger<GlobalExceptionHandler> logger
    )
    {
        _logger = logger;
    }

    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext,
        Exception exception,
        CancellationToken cancellationToken
    )
    {
        _logger.LogError(
            exception,
            "Unhandled exception occured."
        );

        var StatusCode = exception switch
        {
            NotFoundException =>
                StatusCodes.Status404NotFound,

            ConflictException =>
                StatusCodes.Status409Conflict,

            _ =>
                StatusCodes.Status500InternalServerError

        };

        var title = exception switch
        {
            NotFoundException =>
                "Resource not found.",

            ConflictException =>
                "Resource conflict.",

            _ =>
               "An unexpected error occurred."
        };



        var problemDetails = new ProblemDetails
        {
            Status = StatusCode,
            Title = title
        };


        if (exception is NotFoundException ||
        exception is ConflictException)
        {
            problemDetails.Detail = exception.Message;
        }
        else
        {
            problemDetails.Detail =
                "An internal server error occurred.";
        }

        httpContext.Response.StatusCode =
           StatusCodes.Status500InternalServerError;

        await httpContext.Response.WriteAsJsonAsync(
            problemDetails,
            cancellationToken);

        return true;
    }
}
