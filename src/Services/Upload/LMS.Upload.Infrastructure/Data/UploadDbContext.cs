using LMS.Upload.Application.Interfaces;
using LMS.Upload.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace LMS.Upload.Infrastructure.Data;

public class UploadDbContext(DbContextOptions<UploadDbContext> options) : DbContext(options), IUploadDbContext
{
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(UploadDbContext).Assembly);
    }

    public DbSet<FileMetadata> Files => Set<FileMetadata>();
}
