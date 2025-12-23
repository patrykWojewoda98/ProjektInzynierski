using FluentValidation;

namespace ProjektIznynierski.Application.Commands.Country.CreateCountry
{
    public class CreateInvestmentTypeCommandValidation : AbstractValidator<CreateInvestmentTypeCommand>
    {
        public CreateInvestmentTypeCommandValidation()
        {
            RuleFor(x => x.TypeName)
                .NotEmpty().WithMessage("Investment type name is requaired.");


        }
    }
}
