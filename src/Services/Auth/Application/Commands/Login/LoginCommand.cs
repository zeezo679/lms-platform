using AuthService.Application.DTOs;
using MediatR;

namespace AuthService.Application.Commands.Login;

public sealed record LoginCommand(string Email, string Password) : IRequest<AuthResponseDto>;
