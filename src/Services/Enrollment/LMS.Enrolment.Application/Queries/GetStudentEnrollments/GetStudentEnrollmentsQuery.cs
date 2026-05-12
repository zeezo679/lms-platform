using LMS.Enrollment.Application.DTOs;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace LMS.Enrollment.Application.Queries.GetStudentEnrollments
{
    public class GetStudentEnrollmentsQuery : IRequest<IEnumerable<EnrollmentResponseDto>>
    {
        public Guid StudentId { get; set; }

        public GetStudentEnrollmentsQuery(Guid studentId)
        {
            StudentId = studentId;
        }
    }
}
