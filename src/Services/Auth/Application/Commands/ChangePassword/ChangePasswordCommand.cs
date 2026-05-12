using MediatR;

namespace AuthService.Application.Commands.ChangePassword;

public sealed record ChangePasswordCommand(Guid UserId, string CurrentPassword, string NewPassword) : IRequest<Unit>;