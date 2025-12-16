using FluentValidation;

namespace ProjektIznynierski.Application.Commands.Region.UpdateRegion
{
    public class UpdateRegionCommandValidation : AbstractValidator<UpdateRegionCommand>
    {
        public UpdateRegionCommandValidation()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0).WithMessage("Identyfikator regionu jest wymagany i musi być większy od zera.");

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
