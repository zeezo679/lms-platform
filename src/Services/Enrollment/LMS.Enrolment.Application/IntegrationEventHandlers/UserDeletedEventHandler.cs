using LMS.Contracts.Abstractions;
using LMS.Contracts.Events;
using LMS.Enrollment.Application.Interfaces.Repos;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMS.Enrollment.Application.IntegrationEventHandlers
{
    public class UserDeletedEventHandler : IIntegrationEventHandler<UserDeletedIntegrationEvent>
    {
       private readonly IEnrollmentRepository _enrollmentRepository;
       private readonly ILogger<UserDeletedEventHandler> _logger;

       public UserDeletedEventHandler(IEnrollmentRepository enrollmentRepository, ILogger<UserDeletedEventHandler> logger)
       {
           _enrollmentRepository = enrollmentRepository;
           _logger = logger;
       }

       public async Task Handle(UserDeletedIntegrationEvent @event, CancellationToken ct = default)
       {
           _logger.LogInformation($"Received UserDeletedIntegrationEvent for StudentId: {@event.UserId}. Canceling related enrollments.");

           await _enrollmentRepository.CancelEnrollmentsByStudentIdAsync(@event.UserId, ct);

           _logger.LogInformation("Successfully canceled all enrollments for StudentId: {StudentId}.", @event.UserId);
       }
    }
}