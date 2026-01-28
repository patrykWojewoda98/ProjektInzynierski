using FluentValidation;

namespace ProjektIznynierski.Application.Commands.Comment.UpdateComment
{
    public class UpdateCommentCommandValidation : AbstractValidator<UpdateCommentCommand>
    {
        public UpdateCommentCommandValidation()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0)
                .WithMessage("Comment Id must be greater than 0.");

            RuleFor(x => x.Content)
                .NotEmpty()
                .WithMessage("Comment content is required.");
        }
    }
}
