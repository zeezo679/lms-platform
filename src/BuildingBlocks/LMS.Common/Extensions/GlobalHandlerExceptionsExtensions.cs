using System;
using LMS.Common.Middleware;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;

namespace LMS.Common.Extensions;

public static class GlobalHandlerExceptionsExtensions
{
    public static IServiceCollection AddGlobalExceptionHandler(this IServiceCollection services)
    {
        services.AddSingleton<IExceptionHandler, GlobalExceptionHandler>();

        return services;
    }
}
