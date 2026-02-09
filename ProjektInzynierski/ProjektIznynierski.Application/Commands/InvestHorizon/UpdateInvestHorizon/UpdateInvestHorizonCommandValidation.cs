using FluentValidation;

namespace ProjektIznynierski.Application.Commands.InvestHorizon.UpdateInvestHorizon
{
    public class UpdateInvestHorizonCommandValidation : AbstractValidator<UpdateInvestHorizonCommand>
    {
        public UpdateInvestHorizonCommandValidation()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0)
                .WithMessage("Investment horizon identifier is required and must be greater than zero.");

            RuleFor(x => x.Horizon)
                .NotEmpty()
                .WithMessage("Investment horizon is required.")
                .MaximumLength(255)
                .WithMessage("Investment horizon cannot exceed 255 characters.");
        }
    }
}
