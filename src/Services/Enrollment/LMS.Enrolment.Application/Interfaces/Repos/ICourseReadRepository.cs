using LMS.Enrollment.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMS.Enrollment.Application.Interfaces.Repos
{
    public interface ICourseReadRepository
    {
        Task AddCourseAsync(Course course);

        Task<bool> CourseExistsAsync(Guid courseId);

        Task SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}
