using FluentValidation;

namespace ProjektIznynierski.Application.Commands.Wallet.CreateWallet
{
    public class CreateWalletCommandValidation : AbstractValidator<CreateWalletCommand>
    {
        public CreateWalletCommandValidation()
        {
            RuleFor(x => x.ClientId)
                .GreaterThan(0)
                .When(x => x.ClientId.HasValue)
                .WithMessage("Client identifier must be greater than zero.");

            RuleFor(x => x.CashBalance)
                .GreaterThanOrEqualTo(0)
                .WithMessage("Cash balance cannot be negative.");

            RuleFor(x => x.CurrencyId)
                .GreaterThan(0)
                .WithMessage("Currency identifier is required and must be greater than zero.");
        }
    }
}
