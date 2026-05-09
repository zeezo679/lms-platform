namespace LMS.Course.Application.Dtos.RequestDtos;

public class SubmitLessonDto
{
    public Guid LessonId { get; set; }
    public string FileUrl { get; set; } = string.Empty;
}
