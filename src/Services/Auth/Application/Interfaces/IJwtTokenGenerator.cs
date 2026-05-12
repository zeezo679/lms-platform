using Domain.Entities;

namespace AuthService.Application.Interfaces;

/// <summary>
/// Generates access and refresh tokens for authenticated users.
/// </summary>
public interface IJwtTokenGenerator
{
    /// <summary>
    /// Generates a signed JWT access token for the specified user.
    /// </summary>
    /// <param name="user">The user to generate a token for.</param>
    /// <returns>A signed JWT access token string.</returns>
    string GenerateAccessToken(User user);

    /// <summary>
    /// Generates a secure refresh token string.
    /// </summary>
    /// <returns>A refresh token string.</returns>
    string GenerateRefreshToken();
}