using FluentValidation;
using ProjektIznynierski.Application.Commands.RegionCode.UpdateRegionCode;
using ProjektIznynierski.Domain.Abstractions;

namespace ProjektIznynierski.Application.Commands.RegionCode.UpdateRegionCode
{
    public class UpdateRegionCodeCommandValidator
        : AbstractValidator<UpdateRegionCodeCommand>
    {
        public UpdateRegionCodeCommandValidator(
            IRegionCodeRepository regionCodeRepository)
        {
            RuleFor(x => x.Id)
                .GreaterThan(0)
                .WithMessage("Id must be greater than 0.");

            RuleFor(x => x.Code)
                .NotEmpty()
                .WithMessage("Code is required.")
                .MaximumLength(4)
                .WithMessage("Code cannot exceed 4 characters.");
        }
    }
}
