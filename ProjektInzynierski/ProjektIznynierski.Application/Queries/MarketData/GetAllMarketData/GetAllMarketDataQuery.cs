using MediatR;
using ProjektIznynierski.Application.Dtos;
using System.Collections.Generic;

namespace ProjektIznynierski.Application.Queries.MarketData.GetAllMarketData
{
    public class GetAllMarketDataQuery : IRequest<List<MarketDataDto>>
    {
    }
}
