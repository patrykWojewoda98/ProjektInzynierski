using FluentValidation;

namespace ProjektIznynierski.Application.Commands.WalletInstrument.CreateWalletInstrument
{
    public class CreateWalletInstrumentCommandValidation : AbstractValidator<CreateWalletInstrumentCommand>
    {
        public CreateWalletInstrumentCommandValidation()
        {
            RuleFor(x => x.WalletId)
                .GreaterThan(0)
                .WithMessage("Wallet identifier is required and must be greater than zero.");

            RuleFor(x => x.InvestInstrumentId)
                .GreaterThan(0)
                .WithMessage("Investment instrument identifier is required and must be greater than zero.");

            RuleFor(x => x.Quantity)
                .GreaterThan(0)
                .WithMessage("Quantity must be greater than zero.");
        }
    }
}
