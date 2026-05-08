using LMS.Upload.Application.Interfaces;
using LMS.Upload.Application.Options;
using LMS.Upload.Infrastructure.Data;
using LMS.Upload.Infrastructure.Storage;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace LMS.Upload.Infrastructure.Extensions;

public static class InfrastructureExtensions
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<UploadDbContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

        services.AddScoped<IUploadDbContext>(sp => sp.GetRequiredService<UploadDbContext>());
        services.Configure<StorageOptions>(configuration.GetSection("Storage"));
        services.AddScoped<IStorageProvider, LocalStorageProvider>();

        return services;
    }
}
