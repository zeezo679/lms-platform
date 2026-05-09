using LMS.Upload.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace LMS.Upload.Application.Interfaces;

public interface IUploadDbContext
{
    DbSet<FileMetadata> Files { get; }
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
