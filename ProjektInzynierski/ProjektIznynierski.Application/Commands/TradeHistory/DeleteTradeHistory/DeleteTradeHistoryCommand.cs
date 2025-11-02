using MediatR;

namespace ProjektIznynierski.Application.Commands.TradeHistory.DeleteTradeHistory
{
    public class DeleteTradeHistoryCommand : IRequest<Unit>
    {
        public int Id { get; set; }
    }
}
