using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogTalks.Application.Comments.Commands
{
    public class AddValidator : AbstractValidator<AddComand>
    {
        public AddValidator()
        {
            RuleFor(x => x.Text)
                .NotEmpty().WithMessage("Text is required.")
                .MaximumLength(500).WithMessage("Text cannot exceed 500 characters.");

        }
    }
}
