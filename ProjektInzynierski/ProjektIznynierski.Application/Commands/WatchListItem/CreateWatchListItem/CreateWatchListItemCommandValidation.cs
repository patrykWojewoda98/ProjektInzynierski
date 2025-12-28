using FluentValidation;

namespace ProjektIznynierski.Application.Commands.WatchListItem.CreateWatchListItem
{
    public class CreateWatchListItemCommandValidation : AbstractValidator<CreateWatchListItemCommand>
    {
        public CreateWatchListItemCommandValidation()
        {
            RuleFor(x => x.ClientId)
                .GreaterThan(0).WithMessage("Identyfikator klienta jest wymagany i musi być większy od zera.");

            RuleFor(x => x.InvestInstrumentId)
                .GreaterThan(0).WithMessage("Identyfikator instrumentu jest wymagany i musi być większy od zera.");

            
        }
    }
}
