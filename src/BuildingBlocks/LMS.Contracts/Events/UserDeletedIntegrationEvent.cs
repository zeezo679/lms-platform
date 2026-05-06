using LMS.Contracts.Abstractions;

namespace LMS.Contracts.Events;

public sealed record UserDeletedIntegrationEvent(
    Guid UserId,
    Guid EventId,
    DateTime OccurredOn) : IIntegrationEvent
{
    public UserDeletedIntegrationEvent(Guid userId)
        : this(userId, Guid.NewGuid(), DateTime.UtcNow)
    {
    }
}