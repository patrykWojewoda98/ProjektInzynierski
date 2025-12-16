using FluentValidation;

namespace ProjektIznynierski.Application.Commands.Currency.CreateCurrency
{
    public class CreateCurrencyCommandValidation : AbstractValidator<CreateCurrencyCommand>
    {
        public CreateCurrencyCommandValidation()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Nazwa waluty jest wymagana.")
                .MaximumLength(100).WithMessage("Nazwa waluty nie może przekraczać 100 znaków.");

            RuleFor(x => x.CurrencyRiskLevelId)
                .GreaterThan(0).WithMessage("CurrencyRiskLevelId can't be 0 or less.");
        }
    }
}
