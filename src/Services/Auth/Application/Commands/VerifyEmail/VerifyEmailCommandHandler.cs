using AuthService.Application.Interfaces;
using LMS.Common.Exceptions;
using LMS.Contracts.Events;
using LMS.EventBus.Abstractions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace AuthService.Application.Commands.VerifyEmail;

public sealed class VerifyEmailCommandHandler(
    IAppDbContext context,
    IEventBus eventBus) : IRequestHandler<VerifyEmailCommand, Unit>
{
    public async Task<Unit> Handle(VerifyEmailCommand request, CancellationToken cancellationToken)
    {
        var user = await context.Users
            .FirstOrDefaultAsync(u => u.EmailVerificationToken == request.Token, cancellationToken);

        if (user is null)
        {
            throw new DomainNotFoundException("User", request.Token);
        }

        user.VerifyEmail();

        await context.SaveChangesAsync(cancellationToken);

        await eventBus.PublishAsync(
            new EmailVerifiedIntegrationEvent(user.Id, user.Email),
            cancellationToken);

        return Unit.Value;
    }
}