using LMS.Common.Exceptions;
using LMS.Contracts.Events.Enrollments;
using LMS.Enrollment.Application.Interfaces.Repos;
using LMS.EventBus.Abstractions;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace LMS.Enrollment.Application.Commands.UnenrollStudent
{
    public class UnenrollStudentCommandHandler : IRequestHandler<UnenrollStudentCommand, bool>
    {
        private readonly IEnrollmentRepository _enrollmentRepository;
        private readonly IEventBus _eventBus;

        public UnenrollStudentCommandHandler(IEnrollmentRepository enrollmentRepository , IEventBus eventBus)
        {
            _enrollmentRepository = enrollmentRepository;
            _eventBus = eventBus;
        }

        public async Task<bool> Handle(UnenrollStudentCommand request, CancellationToken cancellationToken)
        {
            // 1. Ensure the student is enrolled in the course
            var enrollment = await _enrollmentRepository.GetEnrollmentAsync(request.StudentId, request.CourseId);

            if (enrollment == null)
                throw new DomainNotFoundException($"Student is not enrolled in this course.");

            // 2. Delete the enrollment
            _enrollmentRepository.Delete(enrollment);
            await _enrollmentRepository.SaveChangesAsync();

            // 3. Publish an event to notify other services about the unenrollment
            var @event = new StudentUnenrolledIntegrationEvent(request.StudentId, request.CourseId);
            await _eventBus.PublishAsync(@event, cancellationToken);

            return true;
        }
    }
}
