using FluentValidation;

namespace ProjektIznynierski.Application.Commands.Comment.AddComment
{
    public class AddCommentCommandValidation : AbstractValidator<AddCommentCommand>
    {
        public AddCommentCommandValidation()
        {
            RuleFor(x => x.ClientId)
                .GreaterThan(0)
                .WithMessage("ClientId must be greater than 0.");

            RuleFor(x => x.InvestInstrumentId)
                .GreaterThan(0)
                .WithMessage("InvestInstrumentId must be greater than 0.");

            RuleFor(x => x.Content)
                .NotEmpty()
                .WithMessage("Comment content is required.");
        }
    }
}
