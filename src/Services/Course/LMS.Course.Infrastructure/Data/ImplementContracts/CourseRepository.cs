using LMS.Course.Application.Abstractions;
using LMS.Course.Domain.Entities;
using LMS.Course.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace LMS.Course.Infrastructure.Data.ImplementContracts
{
    public class CourseRepository : ICourseRepository
    {
        private readonly CourseAppDbContext _context;
        public CourseRepository(CourseAppDbContext context)
        {
            _context = context;
        }

        public async Task<CourseEntity?> GetByIdAsync(Guid id, CancellationToken ct = default)
        {
            return await _context.Courses.Include(c => c.Sections)
                            .ThenInclude(s => s.Lessons)
                            .FirstOrDefaultAsync(c => c.Id == id, ct);
        }
        public async Task<(IReadOnlyList<CourseEntity> items, int totalCount)> GetPagedAsync(
            int page, 
            int pageSize, 
            string? category = null, 
            CourseLevel? level = null, 
            string? search = null, 
            CancellationToken ct = default)
        {
            var query = _context.Courses.Include(c => c.Sections)
                            .ThenInclude(s => s.Lessons)
                            .AsNoTracking()
                            .AsQueryable();

            // Filters
            if (!string.IsNullOrWhiteSpace(category))
                query = query.Where(c =>
                        c.Category != null &&
                        c.Category.ToLower() == category.ToLower());

            if (level.HasValue)
                query = query.Where(c => c.Level == level.Value);

            if (!string.IsNullOrWhiteSpace(search))
            {
                var term = search.ToLower();
                query = query.Where(c =>
                        c.Title.ToLower().Contains(term) ||
                        c.Description.ToLower().Contains(term));
            }

            var totalCount = await query.CountAsync(ct);

            var items = await query
                .OrderByDescending(c => c.CreatedAt)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (items, totalCount);
        }
        public async Task AddAsync(CourseEntity course, CancellationToken ct = default) => await _context.Courses.AddAsync(course, ct);
        public void Update(CourseEntity course) => _context.Courses.Update(course);
        public void Delete(CourseEntity course) => _context.Courses.Remove(course);

        public async Task<bool> ExistsAsync(Guid id, CancellationToken ct = default)
            => await _context.Courses.AnyAsync(c => c.Id == id, ct);



    }
}
