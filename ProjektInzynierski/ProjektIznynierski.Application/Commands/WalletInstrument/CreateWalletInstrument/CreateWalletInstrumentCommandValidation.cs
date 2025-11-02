using FluentValidation;

namespace ProjektIznynierski.Application.Commands.WalletInstrument.CreateWalletInstrument
{
    public class CreateWalletInstrumentCommandValidation : AbstractValidator<CreateWalletInstrumentCommand>
    {
        public CreateWalletInstrumentCommandValidation()
        {
            RuleFor(x => x.WalletId)
                .GreaterThan(0).WithMessage("Identyfikator portfela jest wymagany i musi być większy od zera.");

            RuleFor(x => x.InvestInstrumentId)
                .GreaterThan(0).WithMessage("Identyfikator instrumentu jest wymagany i musi być większy od zera.");

            RuleFor(x => x.Quantity)
                .GreaterThan(0).WithMessage("Ilość musi być większa od zera.");
        }
    }
}
