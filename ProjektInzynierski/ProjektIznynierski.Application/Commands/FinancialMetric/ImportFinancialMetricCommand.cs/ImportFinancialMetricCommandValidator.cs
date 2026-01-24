using FluentValidation;
using ProjektIznynierski.Application.Commands.FinancialMetric.ImportFinancialIndicators;

namespace ProjektIznynierski.Application.Commands.FinancialMetric.CreateFinancialMetric
{
    public class ImportFinancialMetricCommandValidator : AbstractValidator<ImportFinancialMetricCommand>
    {
        public ImportFinancialMetricCommandValidator()
        {

            RuleFor(x => x.InvestInstrumentId)
                .GreaterThan(0);
        }
    }
}
