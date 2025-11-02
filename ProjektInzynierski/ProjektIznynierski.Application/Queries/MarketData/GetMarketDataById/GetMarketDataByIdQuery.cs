using MediatR;
using ProjektIznynierski.Application.Dtos;

namespace ProjektIznynierski.Application.Queries.MarketData.GetMarketDataById
{
    public record GetMarketDataByIdQuery(int id) : IRequest<MarketDataDto>
    {
    }
}
