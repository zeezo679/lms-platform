using System;
using Application.Interfaces;
using LMS.Common.Exceptions;
using MediatR;

namespace Application.Commands.UpdateUserAvatar;

public class UpdateUserAvatarCommandHandler(IUserRepository userRepository) : IRequestHandler<UpdateUserAvatarCommand>
{
    public async Task Handle(UpdateUserAvatarCommand request, CancellationToken cancellationToken)
    {
        var userProfile = await userRepository.GetByAuthUserIdAsync(request.AuthUserId, cancellationToken);

        if (userProfile == null)
            throw new DomainNotFoundException("UserProfile", request.AuthUserId);

        userProfile.UpdateAvatar(request.AvatarUrl);
        userRepository.Update(userProfile);
        
        await userRepository.SaveChangesAsync(cancellationToken);
    }
}
