using MediatR;
using ProjektIznynierski.Application.Dtos;

namespace ProjektIznynierski.Application.Commands.TradeHistory.UpdateTradeHistory
{
    public class UpdateTradeHistoryCommand : IRequest<TradeHistoryDto>
    {
        public int Id { get; set; }
        public int WalletId { get; set; }
        public int InvestInstrumentId { get; set; }
        public decimal Quantity { get; set; }
        public decimal Price { get; set; }
        public int Type { get; set; }
        public DateTime TradeDate { get; set; }
    }
}
