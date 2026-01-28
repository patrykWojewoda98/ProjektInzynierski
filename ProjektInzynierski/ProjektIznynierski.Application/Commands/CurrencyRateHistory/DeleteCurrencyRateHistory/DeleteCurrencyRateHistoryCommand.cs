using MediatR;

namespace ProjektIznynierski.Application.Commands.CurrencyRateHistory.DeleteCurrencyRateHistory
{
    public class DeleteCurrencyRateHistoryCommand : IRequest<Unit>
    {
        public int Id { get; set; }
    }
}
