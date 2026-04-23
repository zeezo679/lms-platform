using AuthService.Application.Interfaces;
using LMS.Common.Exceptions;
using LMS.Contracts.Events;
using LMS.EventBus.Abstractions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace AuthService.Application.Commands.ChangePassword;

public sealed class ChangePasswordCommandHandler(
    IAppDbContext context,
    IPasswordHasher passwordHasher,
    IEventBus eventBus) : IRequestHandler<ChangePasswordCommand, Unit>
{
    public async Task<Unit> Handle(ChangePasswordCommand request, CancellationToken cancellationToken)
    {
        var user = await context.Users
            .FirstOrDefaultAsync(u => u.Id == request.UserId, cancellationToken);

        if (user is null)
        {
            throw new DomainNotFoundException("User", request.UserId);
        }

        var isCurrentPasswordValid = passwordHasher.Verify(request.CurrentPassword, user.PasswordHash);
        if (!isCurrentPasswordValid)
        {
            throw new DomainUnauthorizedException("Current password is incorrect.");
        }

        var newPasswordHash = passwordHasher.Hash(request.NewPassword);
        user.ChangePassword(newPasswordHash);
        user.ClearRefreshToken();

        await context.SaveChangesAsync(cancellationToken);

        await eventBus.PublishAsync(
            new PasswordChangedIntegrationEvent(user.Id, user.Email),
            cancellationToken);

        return Unit.Value;
    }
}