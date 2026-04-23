using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Infrastructure.Data;

//Discovered by EF Core tools to create the database context at design time (e.g. for migrations). it will be used when we run the command "dotnet ef migrations add InitialCreate -p Infrastructure -s LMS.Auth.API"
public sealed class AppDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
{
    public AppDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();

        var connectionString =
            Environment.GetEnvironmentVariable("AUTH_DB_CONNECTION")
            ?? "Server=localhost;Database=lms_auth;Password=1234;Port=5432;Username=postgres";

        optionsBuilder.UseNpgsql(connectionString);

        return new AppDbContext(optionsBuilder.Options);
    }
}