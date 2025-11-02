using FluentValidation;

namespace ProjektIznynierski.Application.Commands.Wallet.CreateWallet
{
    public class CreateWalletCommandValidation : AbstractValidator<CreateWalletCommand>
    {
        public CreateWalletCommandValidation()
        {
            RuleFor(x => x.ClientId)
                .GreaterThan(0).When(x => x.ClientId.HasValue).WithMessage("Identyfikator klienta musi być większy od zera.");

            RuleFor(x => x.CashBalance)
                .GreaterThanOrEqualTo(0).WithMessage("Saldo gotówkowe nie może być ujemne.");

            RuleFor(x => x.CurrencyId)
                .GreaterThan(0).WithMessage("Identyfikator waluty jest wymagany i musi być większy od zera.");
        }
    }
}
