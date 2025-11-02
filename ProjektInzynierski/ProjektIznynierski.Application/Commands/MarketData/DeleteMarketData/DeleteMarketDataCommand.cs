using MediatR;

namespace ProjektIznynierski.Application.Commands.MarketData.DeleteMarketData
{
    public class DeleteMarketDataCommand : IRequest<Unit>
    {
        public int Id { get; set; }
    }
}
