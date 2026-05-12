using LMS.Contracts.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMS.Contracts.Events
{
    public record UserProfileUpdatedIntegrationEvent(
        Guid UserId,
        Guid AuthUserId,
        string Email,
        string FullName,
        string Role,
        Guid EventId,
        DateTime OccurredOn) : IIntegrationEvent
    {
        public UserProfileUpdatedIntegrationEvent(
            Guid userId,
            Guid authUserId,
            string email,
            string fullName,
            string role)
            : this(userId, authUserId, email, fullName, role, Guid.NewGuid(), DateTime.UtcNow)
        {
        }
    }
}
