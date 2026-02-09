using FluentValidation;

namespace ProjektIznynierski.Application.Commands.Currency.UpdateCurrency
{
    public class UpdateCurrencyCommandValidation : AbstractValidator<UpdateCurrencyCommand>
    {
        public UpdateCurrencyCommandValidation()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0)
                .WithMessage("Currency identifier is required and must be greater than zero.");

            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Currency name is required.")
                .MaximumLength(100).WithMessage("Currency name cannot exceed 100 characters.");

            RuleFor(x => x.CurrencyRiskLevelId)
                .GreaterThan(0)
                .WithMessage("Currency risk level identifier is required and must be greater than zero.");
        }
    }
}
