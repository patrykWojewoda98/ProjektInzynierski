using MediatR;
using ProjektIznynierski.Application.Dtos;

namespace ProjektIznynierski.Application.Commands.MarketData.UpdateMarketData
{
    public class UpdateMarketDataCommand : IRequest<MarketDataDto>
    {
        public int Id { get; set; }
        public int InvestInstrumentId { get; set; }
        public DateTime Date { get; set; }
        public decimal OpenPrice { get; set; }
        public decimal ClosePrice { get; set; }
        public decimal HighPrice { get; set; }
        public decimal LowPrice { get; set; }
        public long Volume { get; set; }
    }
}
