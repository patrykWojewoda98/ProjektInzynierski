using FluentValidation;

namespace ProjektIznynierski.Application.Commands.WatchList.CreateWatchList
{
    public class CreateWatchListCommandValidation : AbstractValidator<CreateWatchListCommand>
    {
        public CreateWatchListCommandValidation()
        {
            RuleFor(x => x.ClientId)
                .GreaterThan(0)
                .WithMessage("Client identifier is required and must be greater than zero.");

            RuleFor(x => x.CreatedAt)
                .NotEmpty()
                .WithMessage("Creation date is required.");
        }
    }
}
