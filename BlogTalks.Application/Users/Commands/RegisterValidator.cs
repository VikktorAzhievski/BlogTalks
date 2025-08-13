using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogTalks.Application.Users.Commands
{
    public class RegisterValidator : AbstractValidator<RegisterRequest>
    {
        public RegisterValidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email is required.")
                .EmailAddress().WithMessage("Invalid email format.");

            RuleFor(x => x.Password)
                 .NotEmpty().WithMessage("Password")
                 .MinimumLength(6).WithMessage("Password must be at least 6 characters long.")
                 .NotNull().WithMessage("Password cannot be null.");

            RuleFor(x => x.Username).NotEmpty();
        }

    }
}