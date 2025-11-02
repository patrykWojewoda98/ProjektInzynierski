using FluentValidation;

namespace ProjektIznynierski.Application.Commands.WalletInstrument.UpdateWalletInstrument
{
    public class UpdateWalletInstrumentCommandValidation : AbstractValidator<UpdateWalletInstrumentCommand>
    {
        public UpdateWalletInstrumentCommandValidation()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0).WithMessage("Identyfikator pozycji portfela jest wymagany i musi być większy od zera.");

            RuleFor(x => x.WalletId)
                .GreaterThan(0).WithMessage("Identyfikator portfela jest wymagany i musi być większy od zera.");

            RuleFor(x => x.InvestInstrumentId)
                .GreaterThan(0).WithMessage("Identyfikator instrumentu jest wymagany i musi być większy od zera.");

            RuleFor(x => x.Quantity)
                .GreaterThan(0).WithMessage("Ilość musi być większa od zera.");
        }
    }
}
