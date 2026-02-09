using FluentValidation;

namespace ProjektIznynierski.Application.Commands.Country.UpdateCountry
{
    public class UpdateCountryCommandValidation : AbstractValidator<UpdateCountryCommand>
    {
        public UpdateCountryCommandValidation()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0)
                .WithMessage("Country identifier is required and must be greater than zero.");

            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Country name is required.")
                .MaximumLength(150).WithMessage("Country name cannot exceed 150 characters.");

            RuleFor(x => x.IsoCode)
                .NotEmpty().WithMessage("ISO code is required.")
                .Length(2, 3).WithMessage("ISO code must be 2–3 characters long.");

            RuleFor(x => x.RegionId)
                .GreaterThan(0)
                .WithMessage("Region identifier is required and must be greater than zero.");

            RuleFor(x => x.CurrencyId)
                .GreaterThan(0)
                .WithMessage("Currency identifier is required and must be greater than zero.");

            RuleFor(x => x.CountryRiskLevelId)
                .GreaterThan(0)
                .WithMessage("Country risk level identifier is required and must be greater than zero.");
        }
    }
}
