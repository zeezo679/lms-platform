using Application.Dtos;
using Application.Interfaces;
using Domain.Entities;
using Domain.Enums;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Commands.GetOrCreateUser
{
    public class GetOrCreateUserCommandHandler(
        IUserRepository repository,
        ILogger<GetOrCreateUserCommandHandler> logger) : IRequestHandler<GetOrCreateUserCommand, UserProfileDto>
    {
        public async Task<UserProfileDto> Handle(GetOrCreateUserCommand request, CancellationToken ct)
        {
            // If the profile already exists, return it
            var existing = await repository.GetByAuthUserIdAsync(request.AuthUserId, ct);

            if (existing != null)
            {
                logger.LogInformation("UserProfile already exists for AuthUserId {AuthUserId}. Skipping creation.",
                    request.AuthUserId);

                return MapToDto(existing);
            }

            var role = Enum.TryParse<UserRole>(request.Role, ignoreCase: true, out var parsed)
                ? parsed : UserRole.Student;


            // Derive a default first/last name from the email prefix until the user updates it
            var emailPrefix = request.Email.Split('@')[0];

            var profile = UserProfile.Create(
                authUserId: request.AuthUserId,
                email: request.Email,
                firstName: emailPrefix,
                lastName: string.Empty,
                role: role);

            await repository.AddAsync(profile, ct);
            await repository.SaveChangesAsync(ct);

            logger.LogInformation(
                "UserProfile created for AuthUserId {AuthUserId} with email {Email}.",
                    request.AuthUserId, request.Email);

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
