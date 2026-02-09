using FluentValidation;

namespace ProjektIznynierski.Application.Commands.WatchListItem.UpdateWatchListItem
{
    public class UpdateWatchListItemCommandValidation : AbstractValidator<UpdateWatchListItemCommand>
    {
        public UpdateWatchListItemCommandValidation()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0)
                .WithMessage("Watch list item identifier is required and must be greater than zero.");

            RuleFor(x => x.WatchListId)
                .GreaterThan(0)
                .WithMessage("Watch list identifier is required and must be greater than zero.");

            RuleFor(x => x.InvestInstrumentId)
                .GreaterThan(0)
                .WithMessage("Investment instrument identifier is required and must be greater than zero.");

        }
    }
}
