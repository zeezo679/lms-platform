using AuthService.Application.Interfaces;
using LMS.Contracts.Events;
using LMS.EventBus.Abstractions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace AuthService.Application.Commands.ForgotPassword;

public sealed class ForgotPasswordCommandHandler(
    IAppDbContext context,
    IEventBus eventBus) : IRequestHandler<ForgotPasswordCommand, Unit>
{
    public async Task<Unit> Handle(ForgotPasswordCommand request, CancellationToken cancellationToken)
    {
        var normalizedEmail = request.Email.Trim().ToLowerInvariant();

        var user = await context.Users
            .FirstOrDefaultAsync(u => u.Email == normalizedEmail, cancellationToken);

        if (user is null)
        {
            return Unit.Value;
        }

        var passwordResetToken = Guid.NewGuid().ToString();
        user.SetPasswordResetToken(passwordResetToken, DateTime.UtcNow.AddHours(1));

        await context.SaveChangesAsync(cancellationToken);

        await eventBus.PublishAsync(
            new PasswordResetRequestedIntegrationEvent(user.Email, passwordResetToken),
            cancellationToken);

        return Unit.Value;
    }
}