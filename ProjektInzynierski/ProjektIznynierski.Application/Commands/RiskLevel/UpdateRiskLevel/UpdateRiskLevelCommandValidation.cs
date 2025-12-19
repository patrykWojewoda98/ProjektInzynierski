using FluentValidation;
using ProjektIznynierski.Domain.Abstractions;

namespace ProjektIznynierski.Application.Commands.RiskLevel.UpdateRiskLevel
{
    public class UpdateRiskLevelCommandValidation
        : AbstractValidator<UpdateRiskLevelCommand>
    {
        public UpdateRiskLevelCommandValidation(IRiskLevelRepository riskLevelRepository)
        {
            RuleFor(x => x.Id)
                .GreaterThan(0)
                .WithMessage("Id must be greater than 0.");

            RuleFor(x => x.Description)
                .NotEmpty()
                .WithMessage("Description is required.");

            RuleFor(x => x.RiskScale)
                .InclusiveBetween(0, 10)
                .WithMessage("RiskScale must be between 0 and 10.");
        }
    }
}
