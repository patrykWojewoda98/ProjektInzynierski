using MediatR;
using ProjektIznynierski.Application.Dtos;

namespace ProjektIznynierski.Application.Commands.TradeHistory.CreateTradeHistory
{
    public class CreateTradeHistoryCommand : IRequest<TradeHistoryDto>
    {
        public int WalletId { get; set; }
        public int InvestInstrumentId { get; set; }
        public decimal Quantity { get; set; }
        public decimal Price { get; set; }
        public int TradeTypeId { get; set; }
        public DateTime TradeDate { get; set; }
    }
}
