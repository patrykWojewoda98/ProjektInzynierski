using FluentValidation;

namespace ProjektIznynierski.Application.Commands.CurrencyPair.AddCurrencyPair
{
    public class AddCurrencyPairCommandValidation: AbstractValidator<AddCurrencyPairCommand>
    {
        public AddCurrencyPairCommandValidation()
        {
            RuleFor(x => x.BaseCurrencyId)
                .GreaterThan(0)
                .WithMessage("Base currency is required.");

            RuleFor(x => x.QuoteCurrencyId)
                .GreaterThan(0)
                .WithMessage("Quote currency is required.")
                .NotEqual(x => x.BaseCurrencyId)
                .WithMessage("Base and quote currency cannot be the same.");

            RuleFor(x => x.Symbol)
                .NotEmpty()
                .MaximumLength(20)
                .WithMessage("Symbol is required and cannot exceed 20 characters.");
        }
    }
}
