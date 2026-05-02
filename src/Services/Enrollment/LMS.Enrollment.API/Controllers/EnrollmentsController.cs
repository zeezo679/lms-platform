using LMS.Common.Responses;
using LMS.Enrollment.Application.Commands.CreateEnrollment;
using LMS.Enrollment.Application.DTOs;
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
    }
}