using Application.Dtos;
using Application.Interfaces;
using Domain.Entities;
using LMS.Common.Exceptions;
using LMS.Contracts.Events;
using LMS.EventBus.Abstractions;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Commands.UpdateUserProfile
{
    public class UpdateUserProfileCommandHandler(IUserRepository repository, IEventBus eventBus)
        : IRequestHandler<UpdateUserProfileCommand, UserProfileDto>
    {
        public async Task<UserProfileDto> Handle(UpdateUserProfileCommand request, CancellationToken ct)
        {
            var profile = await repository.GetByAuthUserIdAsync(request.AuthUserId, ct);

            if (profile == null)
                throw new DomainNotFoundException("UserProfile", request.AuthUserId);

            profile.UpdateProfile(
                request.FirstName,
                request.LastName,
                request.Bio,
                request.AvatarUrl,
                request.PhoneNumber);

            repository.Update(profile);
            await repository.SaveChangesAsync(ct);

            try
            {
                await eventBus.PublishAsync(
                    new UserProfileUpdatedIntegrationEvent(
                        profile.Id,
                        profile.AuthUserId,
                        profile.Email,
                        profile.FullName,
                        profile.Role.ToString()), ct);
            }
            catch (Exception ex) when (ex is not OperationCanceledException)
            {
                _ = ex;
            }

            return MapToDto(profile);
        }

        private static UserProfileDto MapToDto(UserProfile profile) =>
            new(
                profile.Id,
                profile.AuthUserId,
                profile.Email,
                profile.FirstName,
                profile.LastName,
                profile.FullName,
                profile.Bio,
                profile.AvatarUrl,
                profile.PhoneNumber,
                profile.Role,
                profile.Status,
                profile.CreatedAt,
                profile.LastModifiedAt);
    }
}
