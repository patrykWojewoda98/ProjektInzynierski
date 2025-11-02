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

            RuleFor(x => x.CurrencyRisk)
                .InclusiveBetween(0, 3).WithMessage("Poziom ryzyka waluty musi być poprawną wartością enum.");
        }
    }
}
