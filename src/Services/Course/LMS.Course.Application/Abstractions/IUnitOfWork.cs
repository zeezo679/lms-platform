using System;
using System.Collections.Generic;
using System.Text;

namespace LMS.Course.Application.Abstractions
{
    public interface IUnitOfWork
    {
        Task<int> SaveChangesAsync(CancellationToken ct = default);
    }
}
