using LMS.Contracts.Abstractions;

namespace LMS.Contracts.Events;

public sealed record PasswordChangedIntegrationEvent(
    Guid UserId,
    string Email,
    Guid EventId,
    DateTime OccurredOn) : IIntegrationEvent
{
    public PasswordChangedIntegrationEvent(Guid userId, string email)
        : this(userId, email, Guid.NewGuid(), DateTime.UtcNow)
    {
    }
}