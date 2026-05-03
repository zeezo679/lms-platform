
using LMS.Course.Application.Abstractions;
using LMS.Course.Application.Contracts;
using LMS.Course.Application.Mapping;
using LMS.Course.Application.Services;
using LMS.Course.Infrastructure.Data;
using LMS.Course.Infrastructure.Data.ImplementContracts;
using LMS.Course.Infrastructure.EventBus;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace API
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
            builder.Services.AddOpenApi();
            #region ToBuildSwaggerUI
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            #endregion

            builder.Services.AddDbContext<CourseAppDbContext>(optionBuilder =>
            {
                optionBuilder.UseSqlServer(builder.Configuration.GetConnectionString("constr"));
            });

            builder.Services.AddAutoMapper(typeof(CourseProfile).Assembly);
            builder.Services.AddScoped<IEventPublisher, EventPublisherAdapter>();
            builder.Services.AddScoped<ICourseRepository, CourseRepository>();
            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
            builder.Services.AddScoped<ICourseRepository, CourseRepository>();
            builder.Services.AddScoped<ICourseService, CourseService>();

            // register EventBus (kafka)
            //builder.Services.AddEventBus(builder.Configuration);

            var app = builder.Build();

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

            app.UseAuthorization();


            app.MapControllers();

            await app.RunAsync();
        }
    }
}
