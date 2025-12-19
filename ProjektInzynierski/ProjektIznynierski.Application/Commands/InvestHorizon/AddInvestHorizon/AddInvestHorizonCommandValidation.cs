using FluentValidation;
namespace ProjektIznynierski.Application.Commands.InvestHorizon.AddInvestHorizon
{
    public class AddInvestHorizonCommandValidation : AbstractValidator<AddInvestHorizonCommand>
    {
        public AddInvestHorizonCommandValidation()
        {
            RuleFor(x => x.Horizon)
                .NotEmpty().WithMessage("Horyzont inwestycyjny jest wymagany.")
                .MaximumLength(255).WithMessage("Horyzont inwestycyjny nie może przekraczać 255 znaków.");
        }
    }
}