using FluentValidation;

namespace ProjektIznynierski.Application.Commands.WatchList.UpdateWatchList
{
    public class UpdateWatchListCommandValidation : AbstractValidator<UpdateWatchListCommand>
    {
        public UpdateWatchListCommandValidation()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0)
                .WithMessage("Watch list identifier is required and must be greater than zero.");

            RuleFor(x => x.ClientId)
                .GreaterThan(0)
                .WithMessage("Client identifier is required and must be greater than zero.");

            RuleFor(x => x.CreatedAt)
                .NotEmpty()
                .WithMessage("Creation date is required.");
        }
    }
}
