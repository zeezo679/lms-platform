using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace LMS.Enrollment.Application.Commands.CreateEnrollment
{
    public class CreateEnrollmentCommandValidator : AbstractValidator<CreateEnrollmentCommand>
    {
        public CreateEnrollmentCommandValidator()
        {
            RuleFor(x => x.StudentId)
                .NotEmpty().WithMessage("StudentId is required and cannot be empty.");

            RuleFor(x => x.CourseId)
                .NotEmpty().WithMessage("CourseId is required and cannot be empty.");
        }
    }
}
