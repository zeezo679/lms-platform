using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IUserRepository
    {
        Task<UserProfile?> GetByIdAsync(Guid id, CancellationToken ct = default);
        Task<UserProfile?> GetByAuthUserIdAsync(Guid authUserId, CancellationToken ct = default);
        Task<UserProfile?> GetByEmailAsync(string email, CancellationToken ct = default);
        Task<(IEnumerable<UserProfile> Items, int TotalCount)> GetPagedAsync(int page, int pageSize, CancellationToken ct = default);
        Task<bool> ExistsByAuthUserIdAsync(Guid authUserId, CancellationToken ct = default);
        Task AddAsync(UserProfile user, CancellationToken ct = default);
        void Update(UserProfile user);
        void Delete(UserProfile user);
        Task SaveChangesAsync(CancellationToken ct = default);
    }
}
