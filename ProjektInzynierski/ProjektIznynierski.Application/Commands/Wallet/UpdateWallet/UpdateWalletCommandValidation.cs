using FluentValidation;

namespace ProjektIznynierski.Application.Commands.Wallet.UpdateWallet
{
    public class UpdateWalletCommandValidation : AbstractValidator<UpdateWalletCommand>
    {
        public UpdateWalletCommandValidation()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0).WithMessage("Identyfikator portfela jest wymagany i musi być większy od zera.");

            RuleFor(x => x.ClientId)
                .GreaterThan(0).When(x => x.ClientId.HasValue).WithMessage("Identyfikator klienta musi być większy od zera.");

            RuleFor(x => x.CashBalance)
                .GreaterThanOrEqualTo(0).WithMessage("Saldo gotówkowe nie może być ujemne.");

            RuleFor(x => x.CurrencyId)
                .GreaterThan(0).WithMessage("Identyfikator waluty jest wymagany i musi być większy od zera.");
        }
    }
}
