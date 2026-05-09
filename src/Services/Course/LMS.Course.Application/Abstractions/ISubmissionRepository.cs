using System;

namespace LMS.Course.Application.Abstractions;

public interface ISubmissionRepository
{
    Task AddAsync(Submission submission, CancellationToken ct = default);
    Task<Submission?> GetByIdAsync(Guid id, CancellationToken ct = default);
    Task<IEnumerable<Submission>> GetByLessonIdAsync(Guid lessonId, CancellationToken ct = default);
    Task<IEnumerable<Submission>> GetByStudentIdAsync(Guid studentId, CancellationToken ct = default);
}
