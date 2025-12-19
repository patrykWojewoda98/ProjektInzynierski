using FluentValidation;
using ProjektIznynierski.Application.Commands.Country.CreateCountry;
using ProjektIznynierski.Domain.Abstractions;

namespace ProjektIznynierski.Application.Commands.RiskLevel.CreateRiskLevel
{
    public class CreateRiskLevelCommandValidation
        : AbstractValidator<CreateRiskLevelCommand>
    {
        public CreateRiskLevelCommandValidation(IRiskLevelRepository riskLevelRepository)
        {
            RuleFor(x => x.Description)
                .NotEmpty()
                .WithMessage("Description is required.");

            RuleFor(x => x.RiskScale)
                .InclusiveBetween(0, 10)
                .WithMessage("RiskScale must be between 0 and 10.")
                .Must(value => value % 1 == 0)
                .WithMessage("RiskScale must be an integer.")
                .MustAsync(async (value, cancellationToken) =>
                    !await riskLevelRepository.ExistsByRiskScaleAsync(value))
                .WithMessage("RiskScale must be unique.");
        }
    }
}