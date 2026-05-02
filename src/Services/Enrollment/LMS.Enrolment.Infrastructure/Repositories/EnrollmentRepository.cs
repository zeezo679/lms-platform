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
    }
}
