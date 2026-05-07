using LMS.Contracts.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMS.Contracts.Events
{
    public record UserDeactivatedIntegrationEvent(
        Guid UserId,
        Guid AuthUserId,
        string Email,
        Guid EventId,
        DateTime OccurredOn) : IIntegrationEvent
    {
        public UserDeactivatedIntegrationEvent(Guid userId, Guid authUserId, string email)
            : this(userId, authUserId, email, Guid.NewGuid(), DateTime.UtcNow)
        {
        }
    }
}
