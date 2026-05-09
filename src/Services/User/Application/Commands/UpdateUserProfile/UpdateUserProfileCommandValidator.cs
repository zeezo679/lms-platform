using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Commands.UpdateUserProfile
{
    public class UpdateUserProfileCommandValidator: AbstractValidator<UpdateUserProfileCommand>
    {
        public UpdateUserProfileCommandValidator()
        {
            RuleFor(x => x.FirstName)
                .NotEmpty().WithMessage("First name is required.")
                .MaximumLength(100).WithMessage("First name must not exceed 100 characters.");

            RuleFor(x => x.LastName)
                .MaximumLength(100).WithMessage("Last name must not exceed 100 characters.");

            RuleFor(x => x.Bio)
                .MaximumLength(1000).WithMessage("Bio must not exceed 1000 characters.")
                .When(x => x.Bio is not null);

            RuleFor(x => x.AvatarUrl)
                .MaximumLength(2048).WithMessage("Avatar URL must not exceed 2048 characters.")
                .When(x => x.AvatarUrl is not null);

            RuleFor(x => x.PhoneNumber)
               .MaximumLength(20).WithMessage("Phone number must not exceed 20 characters.")
               .When(x => x.PhoneNumber is not null);
        }
    }
}
