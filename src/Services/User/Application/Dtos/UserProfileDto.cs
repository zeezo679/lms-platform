using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Dtos
{
    public sealed record UserProfileDto(
        Guid Id,
        Guid AuthUserId,
        string Email,
        string FirstName,
        string LastName,
        string FullName,
        string? Bio,
        string? AvatarUrl,
        string? PhoneNumber,
        UserRole Role,
        UserStatus Status,
        DateTime CreatedAt,
        DateTime? LastModifiedAt);
}
