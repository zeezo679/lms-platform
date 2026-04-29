using MediatR;

namespace AuthService.Application.Commands.VerifyEmail;

public sealed record VerifyEmailCommand(string Token) : IRequest<Unit>;