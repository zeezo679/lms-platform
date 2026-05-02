using LMS.Common.Responses;
using LMS.Enrollment.Application.Commands.CreateEnrollment;
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

        [HttpPost]
        public async Task<ActionResult<ApiResponse<EnrollmentResponseDto>>> CreateEnrollment([FromBody] CreateEnrollmentCommand command)
        {
            var result = await _mediator.Send(command);

            return Created(result, "Student enrolled successfully.");
        }

        [HttpGet("student/{studentId}")]
        public async Task<ActionResult<ApiResponse<IEnumerable<EnrollmentResponseDto>>>> GetStudentEnrollments(Guid studentId)
        {
            var query = new GetStudentEnrollmentsQuery(studentId);
            var result = await _mediator.Send(query);

            return Success(result, "Student enrollments retrieved successfully.");
        }

        [HttpGet]
        public async Task<ActionResult<ApiResponse<PagedResponse<StudentEnrollmentsGroupDto>>>> GetAllEnrollments([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            var query = new GetAllEnrollmentsQuery(pageNumber, pageSize);
            var result = await _mediator.Send(query);

            return Success(result, "All enrollments retrieved successfully.");
        }
    }
}