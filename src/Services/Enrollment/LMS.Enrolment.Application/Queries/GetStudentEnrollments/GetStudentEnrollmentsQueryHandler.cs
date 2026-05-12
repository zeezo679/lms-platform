using LMS.Enrollment.Application.DTOs;
using LMS.Enrollment.Application.Interfaces.Repos;
using MediatR;

namespace LMS.Enrollment.Application.Queries.GetStudentEnrollments
{
    public class GetStudentEnrollmentsQueryHandler : IRequestHandler<GetStudentEnrollmentsQuery, IEnumerable<EnrollmentResponseDto>>
    {
        private readonly IEnrollmentRepository _enrollmentRepository;

        public GetStudentEnrollmentsQueryHandler(IEnrollmentRepository enrollmentRepository)
        {
            _enrollmentRepository = enrollmentRepository;
        }

        public async Task<IEnumerable<EnrollmentResponseDto>> Handle(GetStudentEnrollmentsQuery request, CancellationToken cancellationToken)
        {
            // 1. get all enrollments for the student from the repository
            var enrollments = await _enrollmentRepository.GetEnrollmentsByStudentIdAsync(request.StudentId);

            // 2. map from entity to DTO to return to the API
            var response = enrollments.Select(e => new EnrollmentResponseDto
            {
                Id = e.Id,
                StudentId = e.StudentId,
                CourseId = e.CourseId,
                EnrollmentDate = e.EnrollmentDate,
                Status = e.Status.ToString()
            }).ToList();

            return response;
        }
    }
}