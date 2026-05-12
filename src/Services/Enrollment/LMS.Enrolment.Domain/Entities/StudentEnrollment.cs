using LMS.Common.Entities;
using LMS.Enrollment.Domain.Enums;

namespace LMS.Enrollment.Domain.Entities
{
    public class StudentEnrollment : AuditableEntity
    {
        public Guid StudentId { get; set; } 
        public Guid CourseId { get; set; } 
        public DateTime EnrollmentDate { get; set; }
        public EnrollmentStatus Status { get; set; } 
    }
}
