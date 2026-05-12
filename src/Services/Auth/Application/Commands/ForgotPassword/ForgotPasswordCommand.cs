using MediatR;

namespace AuthService.Application.Commands.ForgotPassword;

public sealed record ForgotPasswordCommand(string Email) : IRequest<Unit>;