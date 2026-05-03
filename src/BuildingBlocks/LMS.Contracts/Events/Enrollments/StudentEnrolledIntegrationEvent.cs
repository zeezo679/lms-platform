using LMS.Contracts.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMS.Contracts.Events.Enrollments
{
    public class StudentEnrolledIntegrationEvent : IIntegrationEvent
    {
        public Guid EventId { get; } = Guid.NewGuid();
        public DateTime OccurredOn { get; } = DateTime.UtcNow;

        public Guid EnrollmentId { get; set; }
        public Guid StudentId { get; set; }
        public Guid CourseId { get; set; }
        public StudentEnrolledIntegrationEvent(Guid enrollmentId, Guid studentId, Guid courseId)
        {
            EnrollmentId = enrollmentId;
            StudentId = studentId;
            CourseId = courseId;
        }
    }
}
