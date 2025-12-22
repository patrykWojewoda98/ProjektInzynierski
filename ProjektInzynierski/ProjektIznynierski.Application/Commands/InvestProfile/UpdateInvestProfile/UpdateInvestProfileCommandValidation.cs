using FluentValidation;

namespace ProjektIznynierski.Application.Commands.InvestProfile.UpdateInvestProfile
{
    public class UpdateInvestProfileCommandValidation : AbstractValidator<UpdateInvestProfileCommand>
    {
        public UpdateInvestProfileCommandValidation()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0).WithMessage("Identyfikator profilu inwestycyjnego jest wymagany i musi być większy od zera.");

            RuleFor(x => x.ProfileName)
                .NotEmpty().WithMessage("Nazwa profilu jest wymagana.")
                .MaximumLength(150).WithMessage("Nazwa profilu nie może przekraczać 150 znaków.");

            RuleFor(x => x.AcceptableRiskLevelId)
                .GreaterThan(0).WithMessage("AcceptableRiskLevelId can't be 0 or less.");

            RuleFor(x => x.InvestHorizonId)
                .InclusiveBetween(0, 5).WithMessage("Horyzont inwestycyjny musi być poprawną wartością enum.");

            RuleFor(x => x.TargetReturn)
                .GreaterThanOrEqualTo(0).When(x => x.TargetReturn.HasValue).WithMessage("Docelowy zwrot nie może być ujemny.");

            RuleFor(x => x.MaxDrawDown)
                .GreaterThanOrEqualTo(0).When(x => x.MaxDrawDown.HasValue).WithMessage("Maksymalne obsunięcie nie może być ujemne.");

            RuleFor(x => x.ClientId)
                .GreaterThan(0).When(x => x.ClientId.HasValue).WithMessage("Identyfikator klienta musi być większy od zera.");
        }
    }
}
