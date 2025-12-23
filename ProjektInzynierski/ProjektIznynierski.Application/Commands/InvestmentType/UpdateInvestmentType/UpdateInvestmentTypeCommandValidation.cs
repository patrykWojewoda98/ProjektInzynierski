using FluentValidation;

namespace ProjektIznynierski.Application.Commands.Country.UpdateCountry
{
    public class UpdateInvestmentTypeCommandValidation : AbstractValidator<UpdateInvestmentTypeCommand>
    {
        public UpdateInvestmentTypeCommandValidation()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0).WithMessage("Investment Type Id is required.");

            RuleFor(x => x.TypeName)
                .NotEmpty().WithMessage("Investment Type Type Name is required.");
        }
    }
}
