using AuthService.Application.DTOs;
using AuthService.Application.Interfaces;
using LMS.Common.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace AuthService.Application.Commands.RefreshToken;

public sealed class RefreshTokenCommandHandler(
    IAppDbContext context,
    IJwtTokenGenerator jwtTokenGenerator) : IRequestHandler<RefreshTokenCommand, AuthResponseDto>
{
    public async Task<AuthResponseDto> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
    {
        var user = await context.Users
            .FirstOrDefaultAsync(u => u.RefreshToken == request.RefreshToken, cancellationToken);

        if (user is null)
        {
            throw new DomainUnauthorizedException("Invalid refresh token.");
        }

        if (!user.RefreshTokenExpiry.HasValue || user.RefreshTokenExpiry.Value <= DateTime.UtcNow)
        {
            throw new DomainUnauthorizedException("Refresh token has expired.");
        }

        var accessToken = jwtTokenGenerator.GenerateAccessToken(user);
        var refreshToken = jwtTokenGenerator.GenerateRefreshToken();

        user.SetRefreshToken(refreshToken, DateTime.UtcNow.AddDays(7));

        await context.SaveChangesAsync(cancellationToken);

        return new AuthResponseDto(accessToken, refreshToken, user.Id);
    }
}
