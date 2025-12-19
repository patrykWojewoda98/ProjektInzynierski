using FluentValidation;

namespace ProjektIznynierski.Application.Commands.TradeHistory.CreateTradeHistory
{
    public class CreateTradeHistoryCommandValidation : AbstractValidator<CreateTradeHistoryCommand>
    {
        public CreateTradeHistoryCommandValidation()
        {
            RuleFor(x => x.WalletId)
                .GreaterThan(0).WithMessage("Identyfikator portfela jest wymagany i musi być większy od zera.");

            RuleFor(x => x.InvestInstrumentId)
                .GreaterThan(0).WithMessage("Identyfikator instrumentu jest wymagany i musi być większy od zera.");

            RuleFor(x => x.Quantity)
                .GreaterThan(0).WithMessage("Ilość musi być większa od zera.");

            RuleFor(x => x.Price)
                .GreaterThanOrEqualTo(0).WithMessage("Cena nie może być ujemna.");

            RuleFor(x => x.TradeTypeId)
                .GreaterThan(0).WithMessage("TradeTypeId can't be 0 or less.");

            RuleFor(x => x.TradeDate)
                .NotEmpty().WithMessage("Data transakcji jest wymagana.");
        }
    }
}
