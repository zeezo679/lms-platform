using LMS.Contracts.Abstractions;
using System;
using System.Collections.Generic;
using System.Text;

namespace LMS.Course.Domain.Events
{
    public sealed record CourseDeletedEvent(
        Guid CourseId,
        Guid InstructorId): IIntegrationEvent
    {
        public Guid EventId { get; } = Guid.NewGuid();
        public DateTime CreationDate { get; } = DateTime.UtcNow;
    }
}
