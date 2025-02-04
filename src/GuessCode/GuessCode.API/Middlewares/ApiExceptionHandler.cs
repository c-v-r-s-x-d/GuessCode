﻿using System.ComponentModel.DataAnnotations;
using GuessCode.API.Models;
using Microsoft.AspNetCore.Diagnostics;

namespace GuessCode.API.Middlewares;

public class ApiExceptionHandler : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        switch (exception)
        {
            case ValidationException ex:
                httpContext.Response.StatusCode = StatusCodes.Status400BadRequest;
                await httpContext.Response.WriteAsJsonAsync(BuildErrorResponse(exception), cancellationToken);
                break;
            case not null:
                httpContext.Response.StatusCode = StatusCodes.Status500InternalServerError;
                await httpContext.Response.WriteAsJsonAsync(BuildErrorResponse(exception), cancellationToken);
                break;
        }

        return true;
    }

    private static DefaultExceptionMessage BuildErrorResponse(Exception exception) => new()
    {
        ErrorMessage = exception.Message,
        ErrorSource = exception.Source
    };
}