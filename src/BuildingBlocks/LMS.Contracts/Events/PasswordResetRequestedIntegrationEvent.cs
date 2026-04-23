using LMS.Contracts.Abstractions;

namespace LMS.Contracts.Events;

public sealed record PasswordResetRequestedIntegrationEvent(
    string Email,
    string Token,
    Guid EventId,
    DateTime OccurredOn) : IIntegrationEvent
{
    public PasswordResetRequestedIntegrationEvent(string email, string token)
        : this(email, token, Guid.NewGuid(), DateTime.UtcNow)
    {
    }
}