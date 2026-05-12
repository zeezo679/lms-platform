using LMS.Enrollment.Application.Interfaces.Repos;
using LMS.Enrollment.Domain.Entities;
using LMS.Enrollment.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMS.Enrollment.Infrastructure.Repositories
{
    public class CourseReadRepository : ICourseReadRepository
    {
        private readonly EnrollmentDbContext _dbContext;

        public CourseReadRepository(EnrollmentDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task AddCourseAsync(Course course)
            => await _dbContext.Courses.AddAsync(course);

        public async Task<bool> CourseExistsAsync(Guid courseId)
            => await _dbContext.Courses.AnyAsync(c => c.Id == courseId);
        public async Task SaveChangesAsync(CancellationToken cancellationToken = default)
            => await _dbContext.SaveChangesAsync(cancellationToken);
        
    }
}