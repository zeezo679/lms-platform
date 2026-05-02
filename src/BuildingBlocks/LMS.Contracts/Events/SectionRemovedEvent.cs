using LMS.Contracts.Abstractions;
using System;
using System.Collections.Generic;
using System.Text;

namespace LMS.Contracts.Events;

public sealed record SectionRemovedEvent(
    Guid CourseId,
    Guid SectionID) : IIntegrationEvent
{
    public Guid EventId { get; } = Guid.NewGuid();
    public DateTime CreationDate { get; } = DateTime.UtcNow;

    public DateTime OccurredOn => throw new NotImplementedException();
}
