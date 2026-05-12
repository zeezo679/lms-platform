using LMS.Contracts.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMS.Contracts.Events.Enrollments
{
    public class StudentUnenrolledIntegrationEvent : IIntegrationEvent
    {
        public Guid EventId { get; } = Guid.NewGuid();
        public DateTime OccurredOn { get; } = DateTime.UtcNow;

        public Guid StudentId { get; set; }
        public Guid CourseId { get; set; }

        public StudentUnenrolledIntegrationEvent(Guid studentId, Guid courseId)
        {
            StudentId = studentId;
            CourseId = courseId;
        }
    }
}
