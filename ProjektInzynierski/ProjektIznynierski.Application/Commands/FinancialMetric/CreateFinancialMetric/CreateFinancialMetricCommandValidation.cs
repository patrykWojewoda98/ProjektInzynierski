using FluentValidation;

namespace ProjektIznynierski.Application.Commands.FinancialMetric.CreateFinancialMetric
{
    public class CreateFinancialMetricCommandValidation : AbstractValidator<CreateFinancialMetricCommand>
    {
        public CreateFinancialMetricCommandValidation()
        {
            RuleFor(x => x.PE).GreaterThanOrEqualTo(0).When(x => x.PE.HasValue).WithMessage("Wskaźnik P/E nie może być ujemny.");
            RuleFor(x => x.PB).GreaterThanOrEqualTo(0).When(x => x.PB.HasValue).WithMessage("Wskaźnik P/B nie może być ujemny.");
            RuleFor(x => x.ROE).GreaterThanOrEqualTo(0).When(x => x.ROE.HasValue).WithMessage("Wskaźnik ROE nie może być ujemny.");
            RuleFor(x => x.DebtToEquity).GreaterThanOrEqualTo(0).When(x => x.DebtToEquity.HasValue).WithMessage("Wskaźnik zadłużenia nie może być ujemny.");
            RuleFor(x => x.DividendYield).GreaterThanOrEqualTo(0).When(x => x.DividendYield.HasValue).WithMessage("Stopa dywidendy nie może być ujemna.");
        }
    }
}
