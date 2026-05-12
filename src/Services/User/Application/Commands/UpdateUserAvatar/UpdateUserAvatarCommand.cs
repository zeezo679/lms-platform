using MediatR;

namespace Application.Commands.UpdateUserAvatar;

public record UpdateUserAvatarCommand(Guid AuthUserId, string AvatarUrl) : IRequest;
