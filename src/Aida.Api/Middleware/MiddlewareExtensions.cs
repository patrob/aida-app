using System;
using Microsoft.AspNetCore.Builder;

namespace Aida.Api.Middleware;

public static class MiddlewareExtensions
{
    public static IApplicationBuilder UseExceptionHandling(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<ExceptionHandlingMiddleware>();
    }
} 