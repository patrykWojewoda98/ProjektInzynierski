using FluentValidation;

namespace ProjektIznynierski.Application.Commands.MarketData.UpdateMarketData
{
    public class UpdateMarketDataCommandValidation : AbstractValidator<UpdateMarketDataCommand>
    {
        public UpdateMarketDataCommandValidation()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0).WithMessage("Identyfikator notowania jest wymagany i musi być większy od zera.");

            RuleFor(x => x.InvestInstrumentId)
                .GreaterThan(0).WithMessage("Identyfikator instrumentu jest wymagany i musi być większy od zera.");

            RuleFor(x => x.Date)
                .NotEmpty().WithMessage("Data notowania jest wymagana.");

            RuleFor(x => x.OpenPrice).GreaterThanOrEqualTo(0).WithMessage("Cena otwarcia nie może być ujemna.");
            RuleFor(x => x.ClosePrice).GreaterThanOrEqualTo(0).WithMessage("Cena zamknięcia nie może być ujemna.");
            RuleFor(x => x.HighPrice).GreaterThanOrEqualTo(0).WithMessage("Cena maksymalna nie może być ujemna.");
            RuleFor(x => x.LowPrice).GreaterThanOrEqualTo(0).WithMessage("Cena minimalna nie może być ujemna.");
            RuleFor(x => x.Volume).GreaterThanOrEqualTo(0).WithMessage("Wolumen nie może być ujemny.");
        }
    }
}
