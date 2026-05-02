using FluentValidation;
using LMS.Enrollment.Application.Behaviors;
using LMS.Enrollment.Application.Commands.CreateEnrollment;
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

            // we register FluentValidation validators here, which will automatically discover and register all validators in the assembly
            services.AddValidatorsFromAssembly(typeof(CreateEnrollmentCommandValidator).Assembly);
            services.AddMediatR(cfg => {
                // Register all MediatR handlers from the assembly containing the CreateEnrollmentCommand
                cfg.RegisterServicesFromAssembly(typeof(CreateEnrollmentCommand).Assembly);
                // Register the ValidationBehavior as an open generic type
                cfg.AddOpenBehavior(typeof(ValidationBehavior<,>));
            });
            return services;
        }
    }
}
