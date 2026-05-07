using LMS.Common.Security;
using LMS.Course.Application.Abstractions;
using LMS.Course.Application.Contracts;
using LMS.Course.Application.Mapping;
using LMS.Course.Application.Services;
using LMS.Course.Infrastructure.Data;
using LMS.Course.Infrastructure.Data.ImplementContracts;
using LMS.Course.Infrastructure.EventBus;
using LMS.EventBus.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

namespace LMS.Course.API
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);


            #region ToBuildSwaggerUI
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
            #endregion

            builder.Services.AddDbContext<CourseAppDbContext>(optionBuilder =>
            {
                optionBuilder.UseSqlServer(builder.Configuration.GetConnectionString("constr"));
            });

            builder.Services.AddAutoMapper(cfg => cfg.AddMaps(typeof(CourseProfile).Assembly));

            //Authentication and Authorization
            builder.Services.AddGatewayAuthentication();

            builder.Services.AddScoped<IEventPublisher, EventPublisherAdapter>();
            builder.Services.AddScoped<ICourseRepository, CourseRepository>();
            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
            builder.Services.AddScoped<ICourseService, CourseService>();

            // register EventBus (kafka)
            builder.Services.AddEventBus(builder.Configuration);

            //builder.Services.AddScoped<IEventBus, KafkaEventBus>();

            builder.Services.AddControllers();

            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowAll", policy =>
                {
                    policy.AllowAnyOrigin()
                          .AllowAnyMethod()
                          .AllowAnyHeader();
                });
            });
            var app = builder.Build();
            app.UseCors("AllowAll");

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.MapOpenApi();
                #region ToBuildSwaggerUI
                app.UseSwagger();
                app.UseSwaggerUI();
                #endregion
            }


            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();
            // ==========================================
            // Endpoints Mapping
            // ==========================================
            app.UseAuthentication();
            app.UseAuthorization();
            app.MapControllers();

            await app.RunAsync();
        }
    }
}

