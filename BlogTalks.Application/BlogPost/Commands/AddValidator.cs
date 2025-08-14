using FluentValidation;

namespace BlogTalks.Application.BlogPost.Commands
{
    public class AddValidator : AbstractValidator<AddCommand>
    {
        public AddValidator()
        {
            RuleFor(x => x.Title)
                .NotEmpty().WithMessage("Title is required.");

            RuleFor(x => x.Text)
                .NotEmpty().WithMessage("Text is required.");

            RuleFor(x => x.Tags)
                .NotEmpty().WithMessage("At least one tag is required.");
        }
    }
}