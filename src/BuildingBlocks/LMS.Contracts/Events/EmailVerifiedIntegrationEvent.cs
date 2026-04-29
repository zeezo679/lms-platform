using LMS.Contracts.Abstractions;

namespace LMS.Contracts.Events;

public sealed record EmailVerifiedIntegrationEvent(
    Guid UserId,
    string Email,
    Guid EventId,
    DateTime OccurredOn) : IIntegrationEvent
{
    public EmailVerifiedIntegrationEvent(Guid userId, string email)
        : this(userId, email, Guid.NewGuid(), DateTime.UtcNow)
    {
    }
}