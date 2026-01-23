using MediatR;
using ProjektIznynierski.Application.Dtos;

namespace ProjektIznynierski.Application.Queries.MarketData.GetMarketDataByInvestInstrumentId
{
    public record GetMarketDataByInvestInstrumentIdQuery(int id) : IRequest<List<MarketDataDto>>
    {
    }
}
