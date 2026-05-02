using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace LMS.Enrollment.Application.Dependencies
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            // we register MediatR here, which will automatically discover and register all handlers, requests, and notifications in the assembly
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));

            // if in the future we implement Validation (like FluentValidation) or AutoMapper, we will register them here in the same way

            return services;
        }
    }
}
