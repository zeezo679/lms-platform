using LMS.Contracts.Abstractions;
using LMS.Contracts.Events;
using LMS.Enrollment.Application.Interfaces.Repos;
using LMS.Enrollment.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMS.Enrollment.Application.IntegrationEventHandlers
{
    public class CourseCreatedEventHandler : IIntegrationEventHandler<CourseCreatedEvent>
    {
        private readonly ICourseReadRepository _courseRepository;
        private readonly IEnrollmentRepository _enrollmentRepository;

        public CourseCreatedEventHandler(ICourseReadRepository courseRepository, IEnrollmentRepository enrollmentRepository)
        {
            _courseRepository = courseRepository;
            _enrollmentRepository = enrollmentRepository;
        }

        public async Task Handle(CourseCreatedEvent @event, CancellationToken cancellationToken)
        {
            // 1. insure that the course exists in the read model
            var exists = await _courseRepository.CourseExistsAsync(@event.CourseId);

            if (!exists)
            {
                // 2. add the course in the read model (Memory)
                var newCourse = new Course { Id = @event.CourseId };
                await _courseRepository.AddCourseAsync(newCourse);

                // 3. save the course physically to the Database
                await _courseRepository.SaveChangesAsync(cancellationToken);
            }
        }
    }
}