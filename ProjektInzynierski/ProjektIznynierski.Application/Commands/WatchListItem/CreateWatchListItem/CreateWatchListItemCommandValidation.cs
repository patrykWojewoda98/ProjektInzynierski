using FluentValidation;

namespace ProjektIznynierski.Application.Commands.WatchListItem.CreateWatchListItem
{
    public class CreateWatchListItemCommandValidation : AbstractValidator<CreateWatchListItemCommand>
    {
        public CreateWatchListItemCommandValidation()
        {
            RuleFor(x => x.WatchListId)
                .GreaterThan(0).WithMessage("Identyfikator listy obserwacyjnej jest wymagany i musi być większy od zera.");

            RuleFor(x => x.InvestInstrumentId)
                .GreaterThan(0).WithMessage("Identyfikator instrumentu jest wymagany i musi być większy od zera.");

            RuleFor(x => x.AddedAt)
                .NotEmpty().WithMessage("Data dodania jest wymagana.");
        }
    }
}
