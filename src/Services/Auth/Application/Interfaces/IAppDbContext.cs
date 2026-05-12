using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace AuthService.Application.Interfaces;

public interface IAppDbContext
{
    DbSet<User> Users { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
