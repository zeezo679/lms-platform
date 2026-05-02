using LMS.Common.Exceptions;
using LMS.Enrollment.Application.Interfaces.Repos;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace LMS.Enrollment.Application.Commands.UnenrollStudent
{
    public class UnenrollStudentCommandHandler : IRequestHandler<UnenrollStudentCommand, bool>
    {
        private readonly IEnrollmentRepository _enrollmentRepository;

        public UnenrollStudentCommandHandler(IEnrollmentRepository enrollmentRepository)
        {
            _enrollmentRepository = enrollmentRepository;
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

            return true;
        }
    }
}
