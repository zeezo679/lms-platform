using LMS.Contracts.Abstractions;
using System;
using System.Collections.Generic;
using System.Text;

namespace LMS.Contracts.Events;

public sealed record CourseDeletedEvent(
    Guid CourseId,
    Guid InstructorId): IIntegrationEvent
{
    public Guid EventId { get; } = Guid.NewGuid();
    public DateTime CreationDate { get; } = DateTime.UtcNow;

    public DateTime OccurredOn => throw new NotImplementedException();
}
