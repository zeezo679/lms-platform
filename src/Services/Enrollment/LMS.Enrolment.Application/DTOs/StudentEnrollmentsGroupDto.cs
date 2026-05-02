using System;
using System.Collections.Generic;
using System.Text;

namespace LMS.Enrollment.Application.DTOs
{
    public class StudentEnrollmentsGroupDto
    {
        public Guid StudentId { get; set; }
        public List<Guid> CourseIds { get; set; } = new List<Guid>();
    }
}
