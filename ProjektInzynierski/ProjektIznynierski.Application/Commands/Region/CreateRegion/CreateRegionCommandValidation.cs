using FluentValidation;

namespace ProjektIznynierski.Application.Commands.Region.CreateRegion
{
    public class CreateRegionCommandValidation : AbstractValidator<CreateRegionCommand>
    {
        public CreateRegionCommandValidation()
        {
            RuleFor(x => x.Name)
                .NotEmpty()
                .WithMessage("Region name is required.")
                .MaximumLength(150)
                .WithMessage("Region name cannot exceed 150 characters.");

            RuleFor(x => x.RegionCodeId)
                .InclusiveBetween(1, 10)
                .WithMessage("Region code identifier must be between 1 and 10.");

            RuleFor(x => x.RegionRiskLevelId)
                .GreaterThan(0)
                .WithMessage("Region risk level identifier must be greater than zero.");
        }
    }
}
