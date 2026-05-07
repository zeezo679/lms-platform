using Application.Behaviors;
using Application.Commands.UpdateUserProfile;
using Application.IntegrationEventHandlers;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Application.Dependencies
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            // FluentValidation — scan all validators in this assembly
            services.AddValidatorsFromAssembly(typeof(UpdateUserProfileCommandValidator).Assembly);

            // MediatR — register all handlers + the ValidationBehavior pipeline
            services.AddMediatR(cfg =>
            {
                cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
                cfg.AddOpenBehavior(typeof(ValidationBehavior<,>));
            });


            services.AddTransient<UserRegisteredIntegrationEventHandler>();

            return services;
        }
    }
}
