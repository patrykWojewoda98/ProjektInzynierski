using FluentValidation;
namespace ProjektIznynierski.Application.Commands.InvestHorizon.UpdateInvestHorizon
{
    public class UpdateInvestHorizonCommandValidation : AbstractValidator<UpdateInvestHorizonCommand>
    {
        public UpdateInvestHorizonCommandValidation()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0).WithMessage("Identyfikator horyzontu inwestycyjnego jest wymagany i musi być większy od zera.");
                
            RuleFor(x => x.Horizon)
                .NotEmpty().WithMessage("Horyzont inwestycyjny jest wymagany.")
                .MaximumLength(255).WithMessage("Horyzont inwestycyjny nie może przekraczać 255 znaków.");
        }
    }
}