using AuthService.Application.DTOs;
using MediatR;

namespace AuthService.Application.Commands.RefreshToken;

public sealed record RefreshTokenCommand(string RefreshToken) : IRequest<AuthResponseDto>;
