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
    // الكلاس ده بيطبق الـ Interface بتاع التيم، وبيحدد إنه بيسمع الـ CourseDeletedEvent بس
    public class CourseDeletedEventHandler : IIntegrationEventHandler<CourseDeletedEvent>
    {
        private readonly IEnrollmentRepository _enrollmentRepository;
        private readonly ILogger<CourseDeletedEventHandler> _logger;

        public CourseDeletedEventHandler(IEnrollmentRepository enrollmentRepository, ILogger<CourseDeletedEventHandler> logger)
        {
            _enrollmentRepository = enrollmentRepository;
            _logger = logger;
        }

        public async Task Handle(CourseDeletedEvent @event, CancellationToken ct = default)
        {
            _logger.LogInformation("Received CourseDeletedEvent for CourseId: {CourseId}. Canceling related enrollments.", @event.CourseId);

            await _enrollmentRepository.CancelEnrollmentsByCourseIdAsync(@event.CourseId, ct);
            await _enrollmentRepository.SaveChangesAsync(ct);

            _logger.LogInformation("Successfully canceled all enrollments for CourseId: {CourseId}.", @event.CourseId);
        }
    }
}
