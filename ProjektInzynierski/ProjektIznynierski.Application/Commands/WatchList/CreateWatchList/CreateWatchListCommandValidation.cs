using FluentValidation;

namespace ProjektIznynierski.Application.Commands.WatchList.CreateWatchList
{
    public class CreateWatchListCommandValidation : AbstractValidator<CreateWatchListCommand>
    {
        public CreateWatchListCommandValidation()
        {
            RuleFor(x => x.ClientId)
                .GreaterThan(0).WithMessage("Identyfikator klienta jest wymagany i musi być większy od zera.");

            RuleFor(x => x.CreatedAt)
                .NotEmpty().WithMessage("Data utworzenia jest wymagana.");
        }
    }
}
