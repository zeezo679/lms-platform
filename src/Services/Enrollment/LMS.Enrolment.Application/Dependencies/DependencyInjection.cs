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
            // Add FluentValidation validators from the assembly containing CreateEnrollmentCommandValidator
            services.AddValidatorsFromAssembly(typeof(CreateEnrollmentCommandValidator).Assembly);
            // Add MediatR and register the ValidationBehavior
            services.AddMediatR(cfg => {
                cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
                cfg.AddOpenBehavior(typeof(ValidationBehavior<,>));
            });

            return services;
        }
    }
}