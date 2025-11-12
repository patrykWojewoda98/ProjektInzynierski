using MediatR;
using ProjektIznynierski.Application.Dtos;
using System.Collections.Generic;

namespace ProjektIznynierski.Application.Queries.TradeHistory.GetAllTradeHistories
{
    public class GetAllTradeHistoriesQuery : IRequest<List<TradeHistoryDto>>
    {
    }
}
