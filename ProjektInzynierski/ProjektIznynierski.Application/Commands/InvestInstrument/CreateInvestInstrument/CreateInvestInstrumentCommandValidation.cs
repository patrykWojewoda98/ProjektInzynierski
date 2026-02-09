using FluentValidation;

namespace ProjektIznynierski.Application.Commands.InvestInstrument.CreateInvestInstrument
{
    public class CreateInvestInstrumentCommandValidation : AbstractValidator<CreateInvestInstrumentCommand>
    {
        public CreateInvestInstrumentCommandValidation()
        {
            RuleFor(x => x.Name)
                .NotEmpty()
                .WithMessage("Investment instrument name is required.")
                .MaximumLength(200)
                .WithMessage("Investment instrument name cannot exceed 200 characters.");

            RuleFor(x => x.Ticker)
                .NotEmpty()
                .WithMessage("Ticker is required.")
                .MaximumLength(20)
                .WithMessage("Ticker cannot exceed 20 characters.");

            RuleFor(x => x.InvestmentTypeId)
                .InclusiveBetween(1, 10)
                .WithMessage("Investment type identifier must be between 1 and 10.");

            RuleFor(x => x.Description)
                .MaximumLength(1000)
                .WithMessage("Description cannot exceed 1000 characters.");

            RuleFor(x => x.SectorId)
                .GreaterThan(0)
                .WithMessage("Sector identifier is required and must be greater than zero.");

            RuleFor(x => x.RegionId)
                .GreaterThan(0)
                .WithMessage("Region identifier is required and must be greater than zero.");

            RuleFor(x => x.CountryId)
                .GreaterThan(0)
                .WithMessage("Country identifier is required and must be greater than zero.");

            RuleFor(x => x.CurrencyId)
                .GreaterThan(0)
                .WithMessage("Currency identifier is required and must be greater than zero.");

            RuleFor(x => x.FinancialMetricId)
                .GreaterThan(0)
                .When(x => x.FinancialMetricId.HasValue)
                .WithMessage("Financial metric identifier must be greater than zero.");

            RuleFor(x => x.Isin)
                .NotEmpty()
                .WithMessage("ISIN is required.")
                .Length(12)
                .WithMessage("ISIN must be exactly 12 characters long.");
        }
    }
}
