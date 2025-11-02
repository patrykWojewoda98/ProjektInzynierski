using FluentValidation;

namespace ProjektIznynierski.Application.Commands.WatchList.UpdateWatchList
{
    public class UpdateWatchListCommandValidation : AbstractValidator<UpdateWatchListCommand>
    {
        public UpdateWatchListCommandValidation()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0).WithMessage("Identyfikator listy obserwacyjnej jest wymagany i musi być większy od zera.");

            RuleFor(x => x.ClientId)
                .GreaterThan(0).WithMessage("Identyfikator klienta jest wymagany i musi być większy od zera.");

            RuleFor(x => x.CreatedAt)
                .NotEmpty().WithMessage("Data utworzenia jest wymagana.");
        }
    }
}
