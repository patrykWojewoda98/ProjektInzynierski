using FluentValidation;

namespace ProjektIznynierski.Application.Commands.FinancialMetric.UpdateFinancialMetric
{
    public class UpdateFinancialMetricCommandValidation
        : AbstractValidator<UpdateFinancialMetricCommand>
    {
        public UpdateFinancialMetricCommandValidation()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0)
                .WithMessage("Financial metric identifier is required and must be greater than zero.");

            RuleFor(x => x.PE)
                .GreaterThanOrEqualTo(0)
                .When(x => x.PE.HasValue)
                .WithMessage("P/E ratio cannot be negative.");

            RuleFor(x => x.PB)
                .GreaterThanOrEqualTo(0)
                .When(x => x.PB.HasValue)
                .WithMessage("P/B ratio cannot be negative.");

            RuleFor(x => x.ROE)
                .GreaterThanOrEqualTo(0)
                .When(x => x.ROE.HasValue)
                .WithMessage("ROE value cannot be negative.");

            RuleFor(x => x.DebtToEquity)
                .GreaterThanOrEqualTo(0)
                .When(x => x.DebtToEquity.HasValue)
                .WithMessage("Debt-to-equity ratio cannot be negative.");

            RuleFor(x => x.DividendYield)
                .GreaterThanOrEqualTo(0)
                .When(x => x.DividendYield.HasValue)
                .WithMessage("Dividend yield cannot be negative.");
        }
    }
}
