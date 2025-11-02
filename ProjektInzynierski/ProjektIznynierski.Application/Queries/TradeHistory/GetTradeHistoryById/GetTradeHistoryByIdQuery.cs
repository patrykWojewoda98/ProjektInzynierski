using MediatR;
using ProjektIznynierski.Application.Dtos;

namespace ProjektIznynierski.Application.Queries.TradeHistory.GetTradeHistoryById
{
    public record GetTradeHistoryByIdQuery(int id) : IRequest<TradeHistoryDto>
    {
    }
}
