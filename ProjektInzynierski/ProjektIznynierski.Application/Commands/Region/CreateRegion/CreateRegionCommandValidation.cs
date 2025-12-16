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

            RuleFor(x => x.RegionCodeId)
                .InclusiveBetween(0, 10).WithMessage("RegionCodeId can't be 0 or less");

            RuleFor(x => x.RegionRiskLevelId)
                .GreaterThan(0).WithMessage("RegionRiskLevelId can't be 0 or less.");

        }
    }
}
