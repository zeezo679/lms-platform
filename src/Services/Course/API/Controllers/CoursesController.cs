using LMS.Common.Exceptions;
using LMS.Course.Application.Contracts;
using LMS.Course.Application.Dtos.RequestDtos;
using LMS.Course.Application.Dtos.Response_Dtos;
using LMS.Course.Domain;
using LMS.Course.Domain.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace API.Controllers
{
    [Route("api/courses")]
    [ApiController]
    public class CoursesController : ControllerBase
    {
        private readonly ICourseService _courseService;
        public CoursesController(ICourseService courseService)
        {
            _courseService = courseService;
        }

        // Helpers
        private Guid GetCurrentUserId()
        {
            var claim = User.FindFirst("sub") ?? User.FindFirst("userId");
            return claim is not null && Guid.TryParse(claim.Value, out var id) ? id : Guid.Empty;
        }

        private IActionResult ToErrorResponse(Result result) =>
        result.Error.Code.EndsWith("NotFound")
            ? NotFound(new ProblemDetail(result.Error))
            : BadRequest(new ProblemDetail(result.Error));

        [HttpGet]
        [AllowAnonymous]
        [ProducesResponseType(typeof(PagedResult<CourseSummaryDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetCourses(
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 20,
            [FromQuery] string? category = null,
            [FromQuery] CourseLevel? level = null,
            [FromQuery] string? search = null,
            CancellationToken ct = default)
        {
            var result = await _courseService.GetCoursesAsync(page, pageSize, category, level, search, ct);

            return Ok(result.value);
        }

        [HttpGet("{id:guid}")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(CourseDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetail), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetCourseById(Guid id, CancellationToken ct)
        {
            var result = await _courseService.GetCourseByIdAsync(id, ct);

            return result.IsFailure
                ? NotFound(new ProblemDetail(result.Error))
                : Ok(result.value);
        }

        [HttpPost]
        [Authorize(Roles = "Instructor,Admin")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ProblemDetail), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateCourse([FromBody] CreateCourseDto dto, CancellationToken ct)
        {
            var instructorId = GetCurrentUserId();

            if (instructorId == Guid.Empty)
                throw new DomainUnauthorizedException();

            var result = await _courseService.CreateCourseAsync(dto, instructorId, ct);

            return result.IsFailure
                ? BadRequest(new ProblemDetail(result.Error))
                : CreatedAtAction(nameof(GetCourseById), new { id = result.value }, new { id = result.value });
        }

        [HttpPut("{id:guid}")]
        [Authorize(Roles = "Instructor,Admin")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ProblemDetail), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetail), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateCourse(Guid id, [FromBody] UpdateCourseDto dto, CancellationToken ct)
        {
            var instructorId = GetCurrentUserId();

            if (instructorId == Guid.Empty)
                throw new DomainUnauthorizedException();

            var result = await _courseService.UpdateCourseAsync(id, dto, instructorId, ct);

            return result.IsFailure ? ToErrorResponse(result) : NoContent();
        }

        [HttpDelete("{id:guid}")]
        [Authorize(Roles = "Instructor,Admin")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ProblemDetail), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteCourse(Guid id, CancellationToken ct)
        {
            var instructorId = GetCurrentUserId();

            if (instructorId == Guid.Empty)
                throw new DomainUnauthorizedException();

            var result = await _courseService.DeleteCourseAsync(id, instructorId, ct);

            return result.IsFailure ? ToErrorResponse(result) : NoContent();
        }

        [HttpPost("{id:guid}/publish")]
        [Authorize(Roles = "Instructor,Admin")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ProblemDetail), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetail), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> PublishCourse(Guid id, CancellationToken ct)
        {
            var instructorId = GetCurrentUserId();

            if (instructorId == Guid.Empty)
                throw new DomainUnauthorizedException();

            var result = await _courseService.PublishCourseAsync(id, instructorId, ct);

            return result.IsFailure ? ToErrorResponse(result) : NoContent();
        }


        // Section endpoints
        [HttpPost("{id:guid}/sections")]
        [Authorize(Roles = "Instructor,Admin")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ProblemDetail), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetail), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> AddSection(Guid courseId, [FromBody] AddSectionDto dto, CancellationToken ct)
        {
            var instructorId = GetCurrentUserId();

            if (instructorId == Guid.Empty)
                throw new DomainUnauthorizedException();

            var result = await _courseService.AddSectionAsync(courseId, dto, instructorId, ct);

            return result.IsFailure ? ToErrorResponse(result) : NoContent();
        }

        [HttpDelete("{id:guid}/sections/{sectionId:guid}")]
        [Authorize(Roles = "Instructor,Admin")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ProblemDetail), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> RemoveSection(Guid courseId, Guid sectionId, CancellationToken ct)
        {
            var instructorId = GetCurrentUserId();

            if (instructorId == Guid.Empty)
                throw new DomainUnauthorizedException();

            var result = await _courseService.RemoveSectionAsync(courseId, sectionId, instructorId, ct);

            return result.IsFailure ? ToErrorResponse(result) : NoContent();
        }

        // Lesson endpoints
        [HttpPost("{id:guid}/sections/{sectionId:guid}/lessons")]
        [Authorize(Roles = "Instructor,Admin")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ProblemDetail), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetail), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> AddLesson(Guid courseId, Guid sectionId, [FromBody] AddLessonDto dto, CancellationToken ct)
        {
            var instructorId = GetCurrentUserId();

            if (instructorId == Guid.Empty)
                throw new DomainUnauthorizedException();

            if (dto.SectionId != sectionId)
                return BadRequest(new ProblemDetail(
                    "Lesson.SectionMismatch",
                    "The sectionId in the URL does not match the body."));

            var result = await _courseService.AddLessonAsync(courseId, dto, instructorId, ct);

            return result.IsFailure ? ToErrorResponse(result) : NoContent();
        }

        [HttpPut("{id:guid}/sections/{sectionId:guid}/lessons/{lessonId:guid}")]
        [Authorize(Roles = "Instructor,Admin")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ProblemDetail), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetail), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateLesson(Guid courseId, Guid sectionId, Guid lessonId,
            [FromBody] UpdateLessonDto request, CancellationToken ct)
        {
            var instructorId = GetCurrentUserId();

            if (instructorId == Guid.Empty)
                throw new DomainUnauthorizedException();

            var result = await _courseService.UpdateLessonAsync(
                courseId, sectionId, lessonId, request, instructorId, ct);

            return result.IsFailure ? ToErrorResponse(result) : NoContent();
        }


        [HttpDelete("{id:guid}/sections/{sectionId:guid}/lessons/{lessonId:guid}")]
        [Authorize(Roles = "Instructor,Admin")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ProblemDetail), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> RemoveLesson(Guid courseId, Guid sectionId, Guid lessonId, CancellationToken ct)
        {
            var instructorId = GetCurrentUserId();

            if (instructorId == Guid.Empty)
                throw new DomainUnauthorizedException();

            var result = await _courseService.RemoveLessonAsync(
                courseId, sectionId, lessonId, instructorId, ct);

            return result.IsFailure ? ToErrorResponse(result) : NoContent();
        }


        public record ProblemDetail(string Code, string Description)
        {
            public ProblemDetail(Error error) : this(error.Code, error.Description) { }
        }
    }
}
