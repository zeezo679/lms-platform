using System;
using System.Collections.Generic;
using System.Text;

namespace LMS.Enrollment.Application.DTOs
{
    public class EnrollmentRequestDto
    {
        public Guid StudentId { get; set; }
        public Guid CourseId { get; set; }
    }
}
