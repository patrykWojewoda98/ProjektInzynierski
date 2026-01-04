using FluentValidation;
using ProjektIznynierski.Domain.Abstractions;

namespace ProjektIznynierski.Application.Commands.RegionCode.CreateRegionCode
{
    public class CreateRegionCodeCommandValidator: AbstractValidator<CreateRegionCodeCommand>
    {
        public CreateRegionCodeCommandValidator(
            IRegionCodeRepository regionCodeRepository)
        {
            RuleFor(x => x.Code)
                .NotEmpty()
                .WithMessage("Code is required.")
                .MaximumLength(4)
                .WithMessage("Code cannot exceed 4 characters.");
        }
    }
}