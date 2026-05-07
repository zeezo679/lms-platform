
using Application.Dependencies;
using Application.IntegrationEventHandlers;
using Application.Interfaces;
using Infrastructure.Data;
using Infrastructure.Repositories;
using LMS.Common.Extensions;
using LMS.Contracts.Events;
using LMS.EventBus.Abstractions;
using LMS.EventBus.Extensions;
using Microsoft.EntityFrameworkCore;

namespace API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
            builder.Services.AddOpenApi();

            builder.Services.AddDbContext<UserAppDbContext>(optionBuilder =>
            {
                optionBuilder.UseSqlServer(builder.Configuration.GetConnectionString("constr"));
            });

            builder.Services.AddScoped<IUserRepository, UserRepository>();

            // Application
            builder.Services.AddApplicationServices();

            // Global Exception Handler
            builder.Services.AddGlobalExceptionHandler();

            // Kafka Event Bus 
            builder.Services.AddEventBus(builder.Configuration);

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.MapOpenApi();
            }

            // Event Bus Subscriptions 
            var subscriptionsManager = app.Services.GetRequiredService<LMS.EventBus.Abstractions.IEventBusSubscriptionsManager>();
            subscriptionsManager.AddSubscription<UserRegisteredIntegrationEvent, UserRegisteredIntegrationEventHandler>();

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
