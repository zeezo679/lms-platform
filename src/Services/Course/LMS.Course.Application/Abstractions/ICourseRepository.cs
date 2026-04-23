using LMS.Course.Domain.Entities;
using LMS.Course.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace LMS.Course.Application.Abstractions
{
    public interface ICourseRepository
    {
        Task<CourseEntity?> GetByIdAsync(Guid id, CancellationToken ct = default);
        Task<(IReadOnlyList<CourseEntity> items, int totalCount)> GetPagedAsync(
            int page,
            int pageSize,
            string? category = null,
            CourseLevel? level = null,
            string? search = null,
            CancellationToken ct = default);

        Task AddAsync(CourseEntity course, CancellationToken ct = default);
        void Update(CourseEntity course);
        void Delete(CourseEntity course);
        Task<bool> ExistsAsync(Guid id, CancellationToken ct = default);
    }
}
