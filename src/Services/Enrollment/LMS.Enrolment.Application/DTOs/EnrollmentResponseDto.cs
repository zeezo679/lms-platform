using System;
using System.Collections.Generic;
using System.Text;

namespace LMS.Enrollment.Application.DTOs
{
    public class EnrollmentResponseDto
    {
        public Guid Id { get; set; } 
        public Guid StudentId { get; set; }
        public Guid CourseId { get; set; }
        public DateTime EnrollmentDate { get; set; }
        public string Status { get; set; }
    }
}
