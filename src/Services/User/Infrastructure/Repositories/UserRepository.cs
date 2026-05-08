using Application.Interfaces;
using Domain.Entities;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class UserRepository(UserAppDbContext context) : IUserRepository
    {
        public async Task<UserProfile?> GetByIdAsync(Guid id, CancellationToken ct = default) =>
            await context.UserProfiles.FirstOrDefaultAsync(x => x.Id == id, ct);

        public async Task<UserProfile?> GetByAuthUserIdAsync(Guid authUserId, CancellationToken ct = default) =>
            await context.UserProfiles.FirstOrDefaultAsync(x => x.AuthUserId == authUserId, ct);

        public async Task<UserProfile?> GetByEmailAsync(string email, CancellationToken ct = default) =>
            await context.UserProfiles.FirstOrDefaultAsync(x => x.Email == email, ct);

        public async Task<(IEnumerable<UserProfile> Items, int TotalCount)> GetPagedAsync(int page, int pageSize, CancellationToken ct = default)
        {
            var query = context.UserProfiles.AsNoTracking();

            var totalCount = await query.CountAsync();

            var items = await query
                .OrderByDescending(x => x.CreatedAt)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync(ct);

            return (items, totalCount);
        }

        public Task<bool> ExistsByAuthUserIdAsync(Guid authUserId, CancellationToken ct = default) =>
            context.UserProfiles.AnyAsync(x => x.AuthUserId == authUserId, ct);

        public async Task AddAsync(UserProfile user, CancellationToken ct = default) =>
            await context.UserProfiles.AddAsync(user, ct);

        public void Update(UserProfile user) =>
            context.UserProfiles.Update(user);

        public void Delete(UserProfile user) =>
            context.UserProfiles.Remove(user);

        public Task SaveChangesAsync(CancellationToken ct = default) =>
            context.SaveChangesAsync(ct);
    }
}
