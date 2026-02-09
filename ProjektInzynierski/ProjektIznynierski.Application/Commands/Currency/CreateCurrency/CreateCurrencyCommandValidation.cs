using FluentValidation;

namespace ProjektIznynierski.Application.Commands.Currency.CreateCurrency
{
    public class CreateCurrencyCommandValidation : AbstractValidator<CreateCurrencyCommand>
    {
        public CreateCurrencyCommandValidation()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Currency name is required.")
                .MaximumLength(100).WithMessage("Currency name cannot exceed 100 characters.");

            RuleFor(x => x.CurrencyRiskLevelId)
                .GreaterThan(0)
                .WithMessage("Currency risk level identifier is required and must be greater than zero.");
        }
    }
}
