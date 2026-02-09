using FluentValidation;

namespace ProjektIznynierski.Application.Commands.Sector.UpdateSector
{
    public class UpdateSectorCommandValidation : AbstractValidator<UpdateSectorCommand>
    {
        public UpdateSectorCommandValidation()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0)
                .WithMessage("Sector identifier is required and must be greater than zero.");

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
