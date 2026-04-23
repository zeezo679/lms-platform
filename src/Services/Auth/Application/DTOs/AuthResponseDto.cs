namespace AuthService.Application.DTOs;

/// <summary>
/// Represents authentication tokens and user identity returned by auth flows.
/// </summary>
/// <param name="AccessToken">The signed JWT access token.</param>
/// <param name="RefreshToken">The refresh token used to renew access.</param>
/// <param name="UserId">The authenticated user identifier.</param>
public sealed record AuthResponseDto(string AccessToken, string RefreshToken, Guid UserId);