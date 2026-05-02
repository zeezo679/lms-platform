using LMS.Enrollment.Application.DTOs;
using LMS.Enrollment.Application.Interfaces.Repos;
using LMS.Enrollment.Domain.Entities;
using LMS.Enrollment.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace LMS.Enrollment.Infrastructure.Repositories
{
    public class EnrollmentRepository : IEnrollmentRepository
    {
        private readonly EnrollmentDbContext _context;

        public EnrollmentRepository(EnrollmentDbContext context)
        {
            _context = context;
        }

        public async Task<StudentEnrollment> AddAsync(StudentEnrollment enrollment)
        {
            await _context.Enrollments.AddAsync(enrollment);
            await _context.SaveChangesAsync();
            return enrollment;
        }

        public async Task<IEnumerable<StudentEnrollment>> GetEnrollmentsByStudentIdAsync(Guid studentId)
        {
            return await _context.Enrollments
                .AsNoTracking()
                .Where(e => e.StudentId == studentId)
                .ToListAsync();
        }

        public async Task<bool> HasStudentEnrolledAsync(Guid studentId, Guid courseId)
        {
            return await _context.Enrollments
                 .AsNoTracking()
                 .AnyAsync(e => e.StudentId == studentId && e.CourseId == courseId);
        }

        public async Task<(IEnumerable<StudentEnrollmentsGroupDto> Data, int TotalCount)> GetPagedGroupedEnrollmentsAsync(int pageNumber, int pageSize)
        {
            // 1. get total count of distinct students
            int totalRecords = await _context.Enrollments
                .Select(e => e.StudentId)
                .Distinct()
                .CountAsync();

            // 2. get the student ids for the current page
            var pagedStudentIds = await _context.Enrollments
                .Select(e => e.StudentId)
                .Distinct()
                .OrderBy(id => id)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            // 3. get all enrollments for the paged student ids (this is a single query to the database)
            var enrollments = await _context.Enrollments
                .AsNoTracking()
                .Where(e => pagedStudentIds.Contains(e.StudentId))
                .Select(e => new { e.StudentId, e.CourseId })
                .ToListAsync();

            // 4. we make the grouping in memory to avoid multiple queries to the database
            var data = enrollments
                .GroupBy(e => e.StudentId)
                .Select(g => new StudentEnrollmentsGroupDto
                {
                    StudentId = g.Key,
                    CourseIds = g.Select(e => e.CourseId).ToList()
                })
                .ToList();

            return (data, totalRecords);
        }

        public async Task<StudentEnrollment?> GetEnrollmentAsync(Guid studentId, Guid courseId)
        {
            return await _context.Enrollments
                .FirstOrDefaultAsync(e => e.StudentId == studentId && e.CourseId == courseId);
        }

        public void Delete(StudentEnrollment enrollment)
        {
            _context.Enrollments.Remove(enrollment);
        }

        public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
            =>await _context.SaveChangesAsync(cancellationToken);
    }
}
