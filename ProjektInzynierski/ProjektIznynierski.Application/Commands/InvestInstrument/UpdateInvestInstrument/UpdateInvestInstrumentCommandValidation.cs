using FluentValidation;

namespace ProjektIznynierski.Application.Commands.InvestInstrument.UpdateInvestInstrument
{
    public class UpdateInvestInstrumentCommandValidation : AbstractValidator<UpdateInvestInstrumentCommand>
    {
        public UpdateInvestInstrumentCommandValidation()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0).WithMessage("Identyfikator instrumentu jest wymagany i musi być większy od zera.");

            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Nazwa instrumentu jest wymagana.")
                .MaximumLength(200).WithMessage("Nazwa instrumentu nie może przekraczać 200 znaków.");

            RuleFor(x => x.Ticker)
                .NotEmpty().WithMessage("Ticker jest wymagany.")
                .MaximumLength(20).WithMessage("Ticker nie może przekraczać 20 znaków.");

            RuleFor(x => x.InvestmentTypeId)
                .InclusiveBetween(0, 10).WithMessage("InvestmentTypeId can't be 0 or less");

            RuleFor(x => x.Description)
                .MaximumLength(1000).WithMessage("Opis nie może przekraczać 1000 znaków.");

            RuleFor(x => x.SectorId)
                .GreaterThan(0).WithMessage("Identyfikator sektora jest wymagany i musi być większy od zera.");

            RuleFor(x => x.RegionId)
                .GreaterThan(0).WithMessage("Identyfikator regionu jest wymagany i musi być większy od zera.");

            RuleFor(x => x.CountryId)
                .GreaterThan(0).WithMessage("Identyfikator kraju jest wymagany i musi być większy od zera.");

            RuleFor(x => x.CurrencyId)
                .GreaterThan(0).WithMessage("Identyfikator waluty jest wymagany i musi być większy od zera.");

            RuleFor(x => x.FinancialMetricId)
                .GreaterThan(0).When(x => x.FinancialMetricId.HasValue).WithMessage("Identyfikator metryki finansowej musi być większy od zera.");
        }
    }
}
