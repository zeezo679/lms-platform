using Application.Dtos;
using Application.Interfaces;
using Domain.Entities;
using LMS.Common.Exceptions;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Queries.GetUserByEmail
{
    public class GetUserByEmailQueryHandler(IUserRepository repository)
        : IRequestHandler<GetUserByEmailQuery, UserProfileDto>
    {
        public async Task<UserProfileDto> Handle(GetUserByEmailQuery request, CancellationToken ct)
        {
            var normalizedEmail = request.Email.Trim().ToLowerInvariant();

            var profile = await repository.GetByEmailAsync(normalizedEmail, ct);

            if (profile == null)
                throw new DomainNotFoundException($"No user found with email '{normalizedEmail}'.");

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
