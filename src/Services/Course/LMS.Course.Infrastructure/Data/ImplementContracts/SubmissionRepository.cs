using System;
using LMS.Course.Application.Abstractions;
using Microsoft.EntityFrameworkCore;

namespace LMS.Course.Infrastructure.Data.ImplementContracts;

public class SubmissionRepository : ISubmissionRepository
{
    private readonly CourseAppDbContext _context;

    public SubmissionRepository(CourseAppDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(Submission submission, CancellationToken ct = default)
        => await _context.Submissions.AddAsync(submission, ct);

    public async Task<Submission?> GetByIdAsync(Guid id, CancellationToken ct = default)
        => await _context.Submissions.FirstOrDefaultAsync(s => s.Id == id, ct);

    public async Task<IEnumerable<Submission>> GetByLessonIdAsync(Guid lessonId, CancellationToken ct = default)
        => await _context.Submissions.Where(s => s.LessonId == lessonId).ToListAsync(ct);

    public async Task<IEnumerable<Submission>> GetByStudentIdAsync(Guid studentId, CancellationToken ct = default)
        => await _context.Submissions.Where(s => s.StudentId == studentId).ToListAsync(ct);
}
