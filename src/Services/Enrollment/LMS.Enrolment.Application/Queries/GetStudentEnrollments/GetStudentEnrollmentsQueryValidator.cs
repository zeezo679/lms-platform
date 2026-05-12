using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace LMS.Enrollment.Application.Queries.GetStudentEnrollments
{
    public class GetStudentEnrollmentsQueryValidator : AbstractValidator<GetStudentEnrollmentsQuery>
    {
        public GetStudentEnrollmentsQueryValidator()
        {
            RuleFor(x => x.StudentId)
                .NotEmpty().WithMessage("StudentId is invalid or cannot be empty.");
        }
    }
}
