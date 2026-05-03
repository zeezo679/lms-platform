using LMS.Common.Exceptions;
using LMS.Contracts.Events.Enrollments;
using LMS.Enrollment.Application.DTOs;
using LMS.Enrollment.Application.Interfaces.Repos;
using LMS.Enrollment.Domain.Entities;
using LMS.Enrollment.Domain.Enums;
using LMS.EventBus.Abstractions;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace LMS.Enrollment.Application.Commands.CreateEnrollment
{
    public class CreateEnrollmentCommandHandler : IRequestHandler<CreateEnrollmentCommand, EnrollmentResponseDto>
    {
        private readonly IEnrollmentRepository _enrollmentRepository;
        private readonly IEventBus _eventBus;

        public CreateEnrollmentCommandHandler(IEnrollmentRepository enrollmentRepository, IEventBus eventBus)
        {
            _enrollmentRepository = enrollmentRepository;
            _eventBus = eventBus;
        }
        // this method will handle the command to create an enrollment
        public async Task<EnrollmentResponseDto> Handle(CreateEnrollmentCommand request, CancellationToken cancellationToken)
        {
            // validate the request (Business rules)
            bool alreadyEnrolled = await _enrollmentRepository.HasStudentEnrolledAsync(request.StudentId, request.CourseId);
            if (alreadyEnrolled)
                throw new DomainConflictException("Student is already enrolled in this course.");
            
            // Mapping the request to the domain model
            var enrollment = new StudentEnrollment
            {
                StudentId = request.StudentId,
                CourseId = request.CourseId,
                EnrollmentDate = DateTime.UtcNow,
                Status = EnrollmentStatus.Active
            };
           
            // Save the enrollment to the database
            var savedEnrollment = await _enrollmentRepository.AddAsync(enrollment);
            await _enrollmentRepository.SaveChangesAsync(cancellationToken);
            
            // ToDo : Publish an event to the message bus (e.g., RabbitMQ, Kafka) to notify other services about the new enrollment
            #region Publish Event
            var @event = new StudentEnrolledIntegrationEvent(savedEnrollment.Id, savedEnrollment.StudentId, savedEnrollment.CourseId);
            await _eventBus.PublishAsync(@event, cancellationToken);
            #endregion
           
            // Mapping the saved enrollment to the response DTO
            var response = new EnrollmentResponseDto
            {
                Id = savedEnrollment.Id,
                StudentId = savedEnrollment.StudentId,
                CourseId = savedEnrollment.CourseId,
                EnrollmentDate = savedEnrollment.EnrollmentDate,
                Status = savedEnrollment.Status.ToString()
            };

            return response;
        }
    }
}
