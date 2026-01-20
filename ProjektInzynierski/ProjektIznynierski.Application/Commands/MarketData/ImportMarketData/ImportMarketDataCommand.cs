using MediatR;

namespace ProjektIznynierski.Application.Commands.MarketData.ImportMarketData
{
    public class ImportMarketDataCommand : IRequest<int>
    {
        public string Ticker { get; set; }
    }
}
