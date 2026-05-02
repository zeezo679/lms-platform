using LMS.Enrollment.Application.DTOs;
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

        Task<(IEnumerable<StudentEnrollmentsGroupDto> Data, int TotalCount)> GetPagedGroupedEnrollmentsAsync(int pageNumber, int pageSize);

        Task<StudentEnrollment?> GetEnrollmentAsync(Guid studentId, Guid courseId);

        void Delete(StudentEnrollment enrollment);

        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}
