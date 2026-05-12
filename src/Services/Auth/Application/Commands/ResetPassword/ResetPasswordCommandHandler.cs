using AuthService.Application.Interfaces;
using LMS.Common.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace AuthService.Application.Commands.ResetPassword;

public sealed class ResetPasswordCommandHandler(
    IAppDbContext context,
    IPasswordHasher passwordHasher) : IRequestHandler<ResetPasswordCommand, Unit>
{
    public async Task<Unit> Handle(ResetPasswordCommand request, CancellationToken cancellationToken)
    {
        var user = await context.Users
            .FirstOrDefaultAsync(u => u.PasswordResetToken == request.Token, cancellationToken);

        if (user is null)
        {
            throw new DomainNotFoundException("User", request.Token);
        }

        if (!user.PasswordResetTokenExpiry.HasValue || user.PasswordResetTokenExpiry.Value <= DateTime.UtcNow)
        {
            throw new DomainValidationException("Password reset token has expired.");
        }

        var passwordHash = passwordHasher.Hash(request.NewPassword);

        user.ChangePassword(passwordHash);
        user.ClearPasswordResetToken();
        user.ClearRefreshToken();

        await context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}