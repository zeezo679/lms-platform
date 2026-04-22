using LMS.Contracts.Abstractions;
using System;
using System.Collections.Generic;
using System.Text;

namespace LMS.Course.Domain.Events
{
    public sealed record CourseUpdatedEvent(
        Guid CourseId,
        Guid InstructorId,
        string Title,
        decimal Price) : IIntegrationEvent
    {
        public Guid EventId { get; } = Guid.NewGuid();
        public DateTime CreationDate { get; } = DateTime.UtcNow;
    }
}
