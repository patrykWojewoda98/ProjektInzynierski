using FluentValidation;

namespace ProjektIznynierski.Application.Commands.Sector.UpdateSector
{
    public class UpdateSectorCommandValidation : AbstractValidator<UpdateSectorCommand>
    {
        public UpdateSectorCommandValidation()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0).WithMessage("Identyfikator sektora jest wymagany i musi być większy od zera.");

            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Nazwa sektora jest wymagana.")
                .MaximumLength(150).WithMessage("Nazwa sektora nie może przekraczać 150 znaków.");

            RuleFor(x => x.Code)
                .NotEmpty().WithMessage("Kod sektora jest wymagany.")
                .MaximumLength(50).WithMessage("Kod sektora nie może przekraczać 50 znaków.");

            RuleFor(x => x.Description)
                .MaximumLength(500).WithMessage("Opis nie może przekraczać 500 znaków.");
        }
    }
}
