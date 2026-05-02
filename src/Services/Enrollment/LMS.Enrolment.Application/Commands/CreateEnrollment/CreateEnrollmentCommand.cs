using LMS.Enrollment.Application.DTOs;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace LMS.Enrollment.Application.Commands.CreateEnrollment
{
    public class CreateEnrollmentCommand : IRequest<EnrollmentResponseDto>
    {
        public Guid StudentId { get; set; }
        public Guid CourseId { get; set; }
    }
}
