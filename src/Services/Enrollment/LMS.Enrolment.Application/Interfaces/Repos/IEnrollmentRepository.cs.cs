using LMS.Enrollment.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace LMS.Enrollment.Application.Interfaces.Repos
{
    public interface IEnrollmentRepository
    {
        Task<StudentEnrollment> AddAsync(StudentEnrollment enrollment);

        Task<bool> HasStudentEnrolledAsync(Guid studentId, Guid courseId);

        Task<IEnumerable<StudentEnrollment>> GetEnrollmentsByStudentIdAsync(Guid studentId);
    }
}
