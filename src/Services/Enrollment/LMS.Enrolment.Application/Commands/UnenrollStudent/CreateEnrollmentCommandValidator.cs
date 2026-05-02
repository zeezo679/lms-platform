using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace LMS.Enrollment.Application.Commands.UnenrollStudent
{
    public class UnenrollStudentCommandValidator : AbstractValidator<UnenrollStudentCommand>
    {
        public UnenrollStudentCommandValidator()
        {
            RuleFor(x => x.StudentId)
                .NotEmpty().WithMessage("StudentId is required and cannot be empty.");

            RuleFor(x => x.CourseId)
                .NotEmpty().WithMessage("CourseId is required and cannot be empty.");
        }
    }
}
