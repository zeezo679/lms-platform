using LMS.Common.Exceptions;
using LMS.Common.Responses;
using LMS.Enrollment.Application.Commands.CreateEnrollment;
using LMS.Enrollment.Application.Commands.UnenrollStudent;
using LMS.Enrollment.Application.DTOs;
using LMS.Enrollment.Application.Queries.GetAllEnrollments;
using LMS.Enrollment.Application.Queries.GetStudentEnrollments;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace LMS.Enrollment.API.Controllers
{
    public class EnrollmentsController : AppBaseController
    {
        private readonly IMediator _mediator;

        public EnrollmentsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        // this method is used to extract the student ID from the request headers, which is set by the authentication middleware.
        private Guid GetStudentIdFromHeader()
        {
            var userIdStr = HttpContext.Request.Headers["X-User-Id"].FirstOrDefault();

            if (string.IsNullOrEmpty(userIdStr) || !Guid.TryParse(userIdStr, out Guid studentId))
                throw new UnauthorizedAccessException("User is not authenticated or missing valid ID.");

            return studentId;
        }

        [HttpPost]
        public async Task<ActionResult<ApiResponse<EnrollmentResponseDto>>> CreateEnrollment([FromBody] EnrollmentRequestDto request)
        {
            if (request == null)
                throw new DomainValidationException("Request body is missing or malformed.");

            var studentId = GetStudentIdFromHeader();

            var command = new CreateEnrollmentCommand(studentId, request.CourseId);

            var result = await _mediator.Send(command);

            return Created(result, "Student enrolled successfully.");
        }

        // this endpoint retrieves the enrollments for the authenticated student.
        // It uses the student ID from the request headers to ensure
        // that students can only access their own enrollments.
        [HttpGet("my-enrollments")]
        public async Task<ActionResult<ApiResponse<IEnumerable<EnrollmentResponseDto>>>> GetMyEnrollments()
        {
            var studentId = GetStudentIdFromHeader();

            var query = new GetStudentEnrollmentsQuery(studentId);
            var result = await _mediator.Send(query);

            return Success(result, "Your enrollments retrieved successfully.");
        }

        // this endpoint retrieves all enrollments, grouped by student, with pagination support.
        [HttpGet]
        public async Task<ActionResult<ApiResponse<PagedResponse<StudentEnrollmentsGroupDto>>>> GetAllEnrollments([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            var query = new GetAllEnrollmentsQuery(pageNumber, pageSize);
            var result = await _mediator.Send(query);

            return Success(result, "All enrollments retrieved successfully.");
        }

        // this endpoint allows a student to unenroll from a course.
        // It uses the student ID from the request headers to ensure
        // that students can only unenroll themselves from courses they are enrolled in.
        [HttpDelete("course/{courseId}")]
        public async Task<ActionResult<ApiResponse<string>>> UnenrollStudent(Guid courseId)
        {
            var studentId = GetStudentIdFromHeader();

            var command = new UnenrollStudentCommand(studentId, courseId);
            await _mediator.Send(command);

            return Success("Student unenrolled from the course successfully.");
        }
    }

}