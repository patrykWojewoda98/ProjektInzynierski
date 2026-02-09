using FluentValidation;

namespace ProjektIznynierski.Application.Commands.InvestHorizon.AddInvestHorizon
{
    public class AddInvestHorizonCommandValidation : AbstractValidator<AddInvestHorizonCommand>
    {
        public AddInvestHorizonCommandValidation()
        {
            RuleFor(x => x.Horizon)
                .NotEmpty()
                .WithMessage("Investment horizon is required.")
                .MaximumLength(255)
                .WithMessage("Investment horizon cannot exceed 255 characters.");
        }
    }
}
