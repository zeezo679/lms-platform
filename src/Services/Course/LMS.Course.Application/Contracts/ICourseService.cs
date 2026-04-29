using LMS.Course.Application.Dtos.RequestDtos;
using LMS.Course.Application.Dtos.Response_Dtos;
using LMS.Course.Domain;
using LMS.Course.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace LMS.Course.Application.Contracts
{
    public interface ICourseService
    {
        // Queries
        Task<Result<PagedResult<CourseSummaryDto>>> GetCoursesAsync(
            int page,
            int pageSize,
            string? category,
            CourseLevel? level,
            string? search,
            CancellationToken ct = default);

        Task<Result<CourseDto>> GetCourseByIdAsync(Guid courseId, CancellationToken ct = default);

        // Course commands
        Task<Result<Guid>> CreateCourseAsync(CreateCourseDto dto, Guid instructorId, CancellationToken ct = default);
        Task<Result> UpdateCourseAsync(Guid courseId, UpdateCourseDto dto, Guid requestingInstructorId, CancellationToken ct = default);
        Task<Result> DeleteCourseAsync(Guid courseId, Guid requestingInstructorId, CancellationToken ct = default);
        Task<Result> PublishCourseAsync(Guid courseId, Guid requestingInstructorId, CancellationToken ct = default);

        // Section commands
        Task<Result> AddSectionAsync(Guid courseId, AddSectionDto dto, Guid requestingInstructorId, CancellationToken ct = default);
        Task<Result> RemoveSectionAsync(Guid courseId, Guid sectionId, Guid requestingInstructorId, CancellationToken ct = default);

        // Lesson commands
        Task<Result> AddLessonAsync(Guid courseId, AddLessonDto dto, Guid requestingInstructorId, CancellationToken ct = default);
        Task<Result> UpdateLessonAsync(Guid courseId, Guid sectionId, Guid lessonId, UpdateLessonDto dto, 
            Guid requestingInstructorId, CancellationToken ct = default);
        Task<Result> RemoveLessonAsync(Guid courseId, Guid sectionId, Guid lessonId, 
            Guid requestingInstructorId, CancellationToken ct = default);

    }
}
