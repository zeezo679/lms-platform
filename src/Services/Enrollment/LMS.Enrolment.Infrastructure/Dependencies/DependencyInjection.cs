using LMS.Enrollment.Application.Interfaces.Repos;
using LMS.Enrollment.Infrastructure.Data;
using LMS.Enrollment.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace LMS.Enrollment.Infrastructure.Dependencies
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
        {
            // we register the DbContext here, which will be used to interact with the database
            services.AddDbContext<EnrollmentDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

            // we register the repositories here, which will be used to interact with the database
            // we use AddScoped to open a single connection with the database for each request and close it at the end (better for performance)
            services.AddScoped<IEnrollmentRepository, EnrollmentRepository>();

            // if in the future we implement other services in the Infrastructure (like a class that communicates with an External API or a class for file uploads), we will register them here

            return services;
        }
    }
}
