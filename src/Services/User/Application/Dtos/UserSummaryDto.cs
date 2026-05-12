using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Dtos
{
    public sealed record UserSummaryDto(
        Guid Id,
        Guid AuthUserId,
        string Email,
        string FullName,
        UserRole Role,
        UserStatus Status);
}
