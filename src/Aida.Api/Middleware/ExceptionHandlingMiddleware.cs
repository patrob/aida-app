using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace Aida.Api.Middleware;

public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionHandlingMiddleware> _logger;

    public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An unhandled exception occurred");
            await HandleExceptionAsync(context, ex);
        }
    }

    private static async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        var statusCode = HttpStatusCode.InternalServerError; // Default status code
        object responseObj;

        if (exception is ValidationException validationException)
        {
            statusCode = HttpStatusCode.BadRequest;
            
            // Convert the validation errors to a format that works well with JSON serialization
            var errorDictionary = validationException.Errors
                .GroupBy(x => x.PropertyName)
                .ToDictionary(
                    g => string.IsNullOrEmpty(g.Key) ? "_" : g.Key, 
                    g => g.Select(e => e.ErrorMessage).ToArray()
                );
            
            responseObj = new
            {
                title = "Validation Failure",
                status = (int)statusCode,
                detail = "One or more validation errors occurred",
                errors = errorDictionary
            };
        }
        else if (exception is KeyNotFoundException)
        {
            statusCode = HttpStatusCode.NotFound;
            responseObj = new
            {
                title = "Resource Not Found",
                status = (int)statusCode,
                detail = exception.Message,
                errors = new Dictionary<string, string[]>()
            };
        }
        else
        {
            // Default server error
            responseObj = new
            {
                title = "Server Error",
                status = (int)statusCode,
                detail = exception.Message,
                errors = new Dictionary<string, string[]>()
            };
        }

        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)statusCode;

        var json = JsonSerializer.Serialize(responseObj);
        await context.Response.WriteAsync(json);
    }
} 