using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace LMS.Upload.Infrastructure.Data;

public sealed class UploadDbContextFactory : IDesignTimeDbContextFactory<UploadDbContext>
{
    public UploadDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<UploadDbContext>();

        var connectionString = Environment.GetEnvironmentVariable("UPLOAD_DB_CONNECTION")
            ?? throw new InvalidOperationException("UPLOAD_DB_CONNECTION environment variable is not set.");

        optionsBuilder.UseSqlServer(connectionString);

        return new UploadDbContext(optionsBuilder.Options);
    }
}
