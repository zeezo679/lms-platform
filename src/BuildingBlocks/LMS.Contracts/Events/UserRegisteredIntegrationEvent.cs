using LMS.Contracts.Abstractions;

namespace LMS.Contracts.Events;

public sealed record UserRegisteredIntegrationEvent(
    Guid UserId,
    string Email,
    string VerificationToken,
    string Role,
    Guid EventId,
    DateTime OccurredOn) : IIntegrationEvent
{
    public UserRegisteredIntegrationEvent(Guid userId, string email, string verificationToken, string role)
        : this(userId, email, verificationToken, role, Guid.NewGuid(), DateTime.UtcNow)
    {
    }
}