using FluentValidation;

namespace ProjektIznynierski.Application.Commands.Region.CreateRegion
{
    public class CreateRegionCommandValidation : AbstractValidator<CreateRegionCommand>
    {
        public CreateRegionCommandValidation()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Nazwa regionu jest wymagana.")
                .MaximumLength(150).WithMessage("Nazwa regionu nie może przekraczać 150 znaków.");

            RuleFor(x => x.Code)
                .InclusiveBetween(0, 10).WithMessage("Kod regionu musi być poprawną wartością enum.");

            RuleFor(x => x.RegionRisk)
                .InclusiveBetween(0, 3).WithMessage("Poziom ryzyka regionu musi być poprawną wartością enum.");
        }
    }
}
