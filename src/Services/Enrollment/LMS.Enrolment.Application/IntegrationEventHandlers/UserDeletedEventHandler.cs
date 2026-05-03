using LMS.Contracts.Abstractions;
using LMS.Enrollment.Application.Interfaces.Repos;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMS.Enrollment.Application.IntegrationEventHandlers
{
    //public class UserDeletedEventHandler : IIntegrationEventHandler<UserDeletedIntegrationEvent>
    //{
    //    private readonly IEnrollmentRepository _enrollmentRepository;
    //    private readonly ILogger<UserDeletedEventHandler> _logger;

    //    public UserDeletedEventHandler(IEnrollmentRepository enrollmentRepository, ILogger<UserDeletedEventHandler> logger)
    //    {
    //        _enrollmentRepository = enrollmentRepository;
    //        _logger = logger;
    //    }

    //    public async Task Handle(UserDeletedIntegrationEvent @event, CancellationToken ct = default)
    //    {
    //        _logger.LogInformation("Received UserDeletedIntegrationEvent for StudentId: {StudentId}. Canceling related enrollments.", @event.UserId); // تأكد من اسم الـ Property في الـ Event

    //        await _enrollmentRepository.CancelEnrollmentsByStudentIdAsync(@event.UserId, ct);

    //        _logger.LogInformation("Successfully canceled all enrollments for StudentId: {StudentId}.", @event.UserId);
    //    }
    //}
}