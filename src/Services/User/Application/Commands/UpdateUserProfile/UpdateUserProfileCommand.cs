using Application.Dtos;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Commands.UpdateUserProfile
{
    public record UpdateUserProfileCommand(
        Guid AuthUserId,
        string FirstName,
        string LastName,
        string? Bio,
        string? AvatarUrl,
        string? PhoneNumber) : IRequest<UserProfileDto>;
}
