using AuthService.Application.Interfaces;
using LMS.Common.Exceptions;
using LMS.Contracts.Events;
using LMS.EventBus.Abstractions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace AuthService.Application.Commands.ResendVerificationEmail;

public sealed class ResendVerificationEmailCommandHandler(
    IAppDbContext context,
    IEventBus eventBus) : IRequestHandler<ResendVerificationEmailCommand, Unit>
{
    public async Task<Unit> Handle(ResendVerificationEmailCommand request, CancellationToken cancellationToken)
    {
        var normalizedEmail = request.Email.Trim().ToLowerInvariant();

        var user = await context.Users
            .FirstOrDefaultAsync(u => u.Email == normalizedEmail, cancellationToken);

        if (user is null)
        {
            throw new DomainNotFoundException("User", normalizedEmail);
        }

        if (user.IsEmailVerified)
        {
            throw new DomainValidationException("Email already verified");
        }

        var verificationToken = Guid.NewGuid().ToString();
        user.SetEmailVerificationToken(verificationToken);

        await context.SaveChangesAsync(cancellationToken);

        await eventBus.PublishAsync(
            new UserRegisteredIntegrationEvent(
                user.Id,
                user.Email,
                verificationToken,
                user.Role.ToString()),
            cancellationToken);

        return Unit.Value;
    }
}