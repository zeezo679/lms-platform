using LMS.Course.Application.Abstractions;
using System;
using System.Collections.Generic;
using System.Text;

namespace LMS.Course.Infrastructure.Data.ImplementContracts
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly CourseAppDbContext _context;
        public UnitOfWork(CourseAppDbContext context)
        {
            _context = context;
        }
        public Task<int> SaveChangesAsync(CancellationToken ct = default) => _context.SaveChangesAsync(ct);
    }
}
