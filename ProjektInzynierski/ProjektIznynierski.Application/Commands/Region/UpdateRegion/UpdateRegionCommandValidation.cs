using FluentValidation;

namespace ProjektIznynierski.Application.Commands.Region.UpdateRegion
{
    public class UpdateRegionCommandValidation : AbstractValidator<UpdateRegionCommand>
    {
        public UpdateRegionCommandValidation()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0)
                .WithMessage("Region identifier is required and must be greater than zero.");

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
