using LMS.Common.Entities;
using LMS.Course.Domain.Errors;

public class Submission : AuditableEntity
{
    public Guid LessonId { get; private set; }
    public Guid StudentId { get; private set; }
    public string FileUrl { get; private set; } = string.Empty;
    public DateTime SubmittedAt { get; private set; }
    public double? Grade { get; private set; }

    private Submission() { }

    public static Result<Submission> Create(Guid lessonId, Guid studentId, string fileUrl)
    {
        if (lessonId == Guid.Empty)
            return Result<Submission>.Failure(SubmissionErrors.InvalidLesson);

        if (studentId == Guid.Empty)
            return Result<Submission>.Failure(SubmissionErrors.InvalidStudent);

        if (string.IsNullOrWhiteSpace(fileUrl))
            return Result<Submission>.Failure(SubmissionErrors.FileUrlRequired);

        return Result<Submission>.Success(new Submission
        {
            LessonId = lessonId,
            StudentId = studentId,
            FileUrl = fileUrl.Trim(),
            SubmittedAt = DateTime.UtcNow
        });
    }

    public void SetGrade(double grade)
    {
        Grade = grade;
    }
}