using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace LMS.Enrollment.Application.Commands.UnenrollStudent
{
    public class UnenrollStudentCommand : IRequest<bool>
    {
        public Guid StudentId { get; set; }
        public Guid CourseId { get; set; }

        public UnenrollStudentCommand(Guid studentId, Guid courseId)
        {
            StudentId = studentId;
            CourseId = courseId;
        }
    }
}
