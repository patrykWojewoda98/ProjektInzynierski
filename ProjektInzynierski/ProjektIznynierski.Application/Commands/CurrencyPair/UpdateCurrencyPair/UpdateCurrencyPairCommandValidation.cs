using FluentValidation;

namespace ProjektIznynierski.Application.Commands.CurrencyPair.UpdateCurrencyPair
{
    public class UpdateCurrencyPairCommandValidation : AbstractValidator<UpdateCurrencyPairCommand>
    {
        public UpdateCurrencyPairCommandValidation()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0);

            RuleFor(x => x.BaseCurrencyId)
                .GreaterThan(0);

            RuleFor(x => x.QuoteCurrencyId)
                .GreaterThan(0)
                .NotEqual(x => x.BaseCurrencyId);

            RuleFor(x => x.Symbol)
                .NotEmpty()
                .MaximumLength(20);
        }
    }
}
