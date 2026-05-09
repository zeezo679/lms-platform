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

namespace Application.Queries.GetUserById
{
    public class GetUserByIdQueryHandler(IUserRepository repository)
        : IRequestHandler<GetUserByIdQuery, UserProfileDto>
    {
        public async Task<UserProfileDto> Handle(GetUserByIdQuery request, CancellationToken ct)
        {
            var profile = await repository.GetByIdAsync(request.Id, ct);

            if (profile == null)
                throw new DomainNotFoundException("UserProfile", request.Id);

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
