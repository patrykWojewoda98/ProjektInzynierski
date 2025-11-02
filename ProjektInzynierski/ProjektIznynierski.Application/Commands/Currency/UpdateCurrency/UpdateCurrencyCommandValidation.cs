using FluentValidation;

namespace ProjektIznynierski.Application.Commands.Currency.UpdateCurrency
{
    public class UpdateCurrencyCommandValidation : AbstractValidator<UpdateCurrencyCommand>
    {
        public UpdateCurrencyCommandValidation()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0).WithMessage("Identyfikator waluty jest wymagany i musi być większy od zera.");

            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Nazwa waluty jest wymagana.")
                .MaximumLength(100).WithMessage("Nazwa waluty nie może przekraczać 100 znaków.");

            RuleFor(x => x.CurrencyRisk)
                .InclusiveBetween(0, 3).WithMessage("Poziom ryzyka waluty musi być poprawną wartością enum.");
        }
    }
}
