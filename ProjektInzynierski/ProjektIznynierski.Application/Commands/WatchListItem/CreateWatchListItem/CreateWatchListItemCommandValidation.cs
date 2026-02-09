using FluentValidation;

namespace ProjektIznynierski.Application.Commands.WatchListItem.CreateWatchListItem
{
    public class CreateWatchListItemCommandValidation : AbstractValidator<CreateWatchListItemCommand>
    {
        public CreateWatchListItemCommandValidation()
        {
            RuleFor(x => x.ClientId)
                .GreaterThan(0)
                .WithMessage("Client identifier is required and must be greater than zero.");

            RuleFor(x => x.InvestInstrumentId)
                .GreaterThan(0)
                .WithMessage("Investment instrument identifier is required and must be greater than zero.");
        }
    }
}
