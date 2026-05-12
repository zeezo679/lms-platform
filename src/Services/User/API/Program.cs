
using Application.Dependencies;
using Application.IntegrationEventHandlers;
using Application.Interfaces;
using Infrastructure.Data;
using Infrastructure.Repositories;
using LMS.Common.Extensions;
using LMS.Common.Security;
using LMS.Contracts.Events;
using LMS.EventBus.Abstractions;
using LMS.EventBus.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

namespace API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(options =>
            {
                options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "Enter your JWT token here"
                });

                options.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        Array.Empty<string>()
                    }
                });
            });

            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowAll", policy =>
                    policy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
            });

            builder.Services.AddDbContext<UserAppDbContext>(optionBuilder =>
            {
                optionBuilder.UseSqlServer(builder.Configuration.GetConnectionString("constr"));
            });

            builder.Services.AddScoped<IUserRepository, UserRepository>();

            // Application
            builder.Services.AddApplicationServices();

            builder.Services.AddGatewayAuthentication();

            // Global Exception Handler
            builder.Services.AddGlobalExceptionHandler();

            // Kafka Event Bus 
            builder.Services.AddEventBus(builder.Configuration);

            var app = builder.Build();

            // --- START OF MIGRATION BLOCK ---
            using (var scope = app.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                try
                {
                    var db = services.GetRequiredService<UserAppDbContext>();
                    db.Database.Migrate();
                }
                catch (Exception ex)
                {
                    var logger = services.GetRequiredService<ILogger<Program>>();
                    logger.LogError(ex, "An error occurred while migrating the database.");
                }
            }
            // --- END OF MIGRATION BLOCK ---

            app.UseCors("AllowAll");

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            // Event Bus Subscriptions 
            var subscriptionsManager = app.Services.GetRequiredService<LMS.EventBus.Abstractions.IEventBusSubscriptionsManager>();
            subscriptionsManager.AddSubscription<UserRegisteredIntegrationEvent, UserRegisteredIntegrationEventHandler>();

            app.UseHttpsRedirection();


            app.UseAuthentication();
            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
