using FluentValidation;

namespace ProjektIznynierski.Application.Commands.Country.UpdateCountry
{
    public class UpdateCountryCommandValidation : AbstractValidator<UpdateCountryCommand>
    {
        public UpdateCountryCommandValidation()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0).WithMessage("Identyfikator kraju jest wymagany i musi być większy od zera.");

            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Nazwa kraju jest wymagana.")
                .MaximumLength(150).WithMessage("Nazwa kraju nie może przekraczać 150 znaków.");

            RuleFor(x => x.IsoCode)
                .NotEmpty().WithMessage("Kod ISO jest wymagany.")
                .Length(2, 3).WithMessage("Kod ISO musi mieć długość 2-3 znaki.");

            RuleFor(x => x.RegionId)
                .GreaterThan(0).WithMessage("Identyfikator regionu jest wymagany i musi być większy od zera.");

            RuleFor(x => x.CurrencyId)
                .GreaterThan(0).WithMessage("Identyfikator waluty jest wymagany i musi być większy od zera.");

            RuleFor(x => x.CountryRiskLevelId)
                .GreaterThan(0).WithMessage("CountryRiskLevelId can't be 0 or less.");
        }
    }
}
