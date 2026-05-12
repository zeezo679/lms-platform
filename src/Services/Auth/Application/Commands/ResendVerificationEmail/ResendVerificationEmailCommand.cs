using MediatR;

namespace AuthService.Application.Commands.ResendVerificationEmail;

public sealed record ResendVerificationEmailCommand(string Email) : IRequest<Unit>;