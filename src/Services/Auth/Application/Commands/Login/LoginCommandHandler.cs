using AuthService.Application.DTOs;
using AuthService.Application.Interfaces;
using LMS.Common.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace AuthService.Application.Commands.Login;

public sealed class LoginCommandHandler(
    IAppDbContext context,
    IPasswordHasher passwordHasher,
    IJwtTokenGenerator jwtTokenGenerator) : IRequestHandler<LoginCommand, AuthResponseDto>
{
    public async Task<AuthResponseDto> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        var normalizedEmail = request.Email.Trim().ToLowerInvariant();

        var user = await context.Users
            .FirstOrDefaultAsync(u => u.Email == normalizedEmail, cancellationToken);

        if (user is null)
        {
            throw new DomainNotFoundException("User", normalizedEmail);
        }

        var passwordValid = passwordHasher.Verify(request.Password, user.PasswordHash);
        if (!passwordValid)
        {
            throw new DomainUnauthorizedException("Invalid email or password.");
        }

        if (!user.IsEmailVerified)
        {
            throw new DomainUnauthorizedException("Email not verified");
        }

        var accessToken = jwtTokenGenerator.GenerateAccessToken(user);
        var refreshToken = jwtTokenGenerator.GenerateRefreshToken();

        user.SetRefreshToken(refreshToken, DateTime.UtcNow.AddDays(7));

        await context.SaveChangesAsync(cancellationToken);

        return new AuthResponseDto(accessToken, refreshToken, user.Id);
    }
}
