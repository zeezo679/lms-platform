using MediatR;

namespace AuthService.Application.Commands.ResetPassword;

public sealed record ResetPasswordCommand(string Token, string NewPassword) : IRequest<Unit>;