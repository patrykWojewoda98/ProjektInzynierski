using FluentValidation;

namespace ProjektIznynierski.Application.Commands.Sector.CreateSector
{
    public class CreateSectorCommandValidation : AbstractValidator<CreateSectorCommand>
    {
        public CreateSectorCommandValidation()
        {
            RuleFor(x => x.Name)
                .NotEmpty()
                .WithMessage("Sector name is required.")
                .MaximumLength(150)
                .WithMessage("Sector name cannot exceed 150 characters.");

            RuleFor(x => x.Code)
                .NotEmpty()
                .WithMessage("Sector code is required.")
                .MaximumLength(50)
                .WithMessage("Sector code cannot exceed 50 characters.");

            RuleFor(x => x.Description)
                .MaximumLength(500)
                .WithMessage("Description cannot exceed 500 characters.");
        }
    }
}
`
