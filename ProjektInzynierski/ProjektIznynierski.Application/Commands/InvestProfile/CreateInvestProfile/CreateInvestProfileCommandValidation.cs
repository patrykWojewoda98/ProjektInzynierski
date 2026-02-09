using FluentValidation;

namespace ProjektIznynierski.Application.Commands.InvestProfile.CreateInvestProfile
{
    public class CreateInvestProfileCommandValidation : AbstractValidator<CreateInvestProfileCommand>
    {
        public CreateInvestProfileCommandValidation()
        {
            RuleFor(x => x.ProfileName)
                .NotEmpty()
                .WithMessage("Profile name is required.")
                .MaximumLength(150)
                .WithMessage("Profile name cannot exceed 150 characters.");

            RuleFor(x => x.AcceptableRiskLevelId)
                .GreaterThan(0)
                .WithMessage("Acceptable risk level identifier must be greater than zero.");

            RuleFor(x => x.InvestHorizonID)
                .GreaterThan(0)
                .When(x => x.ClientId.HasValue)
                .WithMessage("Investment horizon identifier must be greater than zero.");

            RuleFor(x => x.TargetReturn)
                .GreaterThanOrEqualTo(0)
                .When(x => x.TargetReturn.HasValue)
                .WithMessage("Target return cannot be negative.");

            RuleFor(x => x.MaxDrawDown)
                .GreaterThanOrEqualTo(0)
                .When(x => x.MaxDrawDown.HasValue)
                .WithMessage("Maximum drawdown cannot be negative.");

            RuleFor(x => x.ClientId)
                .GreaterThan(0)
                .When(x => x.ClientId.HasValue)
                .WithMessage("Client identifier must be greater than zero.");
        }
    }
}
