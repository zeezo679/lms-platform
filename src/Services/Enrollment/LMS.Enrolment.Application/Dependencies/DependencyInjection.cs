using FluentValidation;
using LMS.Contracts.Events;
using LMS.Enrollment.Application.Behaviors;
using LMS.Enrollment.Application.Commands.CreateEnrollment;
using LMS.Enrollment.Application.IntegrationEventHandlers;
using LMS.EventBus.Abstractions;
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
            // transiant because we want a new instance of the handler for each event
            services.AddTransient<CourseDeletedEventHandler>();
            // services.AddTransient<UserDeletedEventHandler>()
            return services;
        }
    }
}