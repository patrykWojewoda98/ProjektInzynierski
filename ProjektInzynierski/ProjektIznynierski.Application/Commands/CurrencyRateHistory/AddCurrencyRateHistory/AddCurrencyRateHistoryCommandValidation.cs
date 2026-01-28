using FluentValidation;

namespace ProjektIznynierski.Application.Commands.CurrencyRateHistory.AddCurrencyRateHistory
{
    public class AddCurrencyRateHistoryCommandValidation : AbstractValidator<AddCurrencyRateHistoryCommand>
    {
        public AddCurrencyRateHistoryCommandValidation()
        {
            RuleFor(x => x.CurrencyPairId)
                .GreaterThan(0)
                .WithMessage("CurrencyPairId is required.");

            RuleFor(x => x.Date)
                .NotEmpty()
                .WithMessage("Date is required.");

            RuleFor(x => x.CloseRate)
                .GreaterThan(0)
                .WithMessage("CloseRate must be greater than zero.");

            RuleFor(x => x.HighRate)
                .GreaterThan(x => x.LowRate)
                .When(x => x.HighRate.HasValue && x.LowRate.HasValue)
                .WithMessage("HighRate must be greater than LowRate.");
        }
    }
}
