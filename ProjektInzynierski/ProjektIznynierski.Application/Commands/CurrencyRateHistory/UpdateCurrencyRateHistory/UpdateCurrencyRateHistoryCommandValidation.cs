using FluentValidation;

namespace ProjektIznynierski.Application.Commands.CurrencyRateHistory.UpdateCurrencyRateHistory
{
    public class UpdateCurrencyRateHistoryCommandValidation : AbstractValidator<UpdateCurrencyRateHistoryCommand>
    {
        public UpdateCurrencyRateHistoryCommandValidation()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0);

            RuleFor(x => x.Date)
                .NotEmpty();

            RuleFor(x => x.CloseRate)
                .GreaterThan(0);

            RuleFor(x => x.HighRate)
                .GreaterThan(x => x.LowRate)
                .When(x => x.HighRate.HasValue && x.LowRate.HasValue);
        }
    }
}
