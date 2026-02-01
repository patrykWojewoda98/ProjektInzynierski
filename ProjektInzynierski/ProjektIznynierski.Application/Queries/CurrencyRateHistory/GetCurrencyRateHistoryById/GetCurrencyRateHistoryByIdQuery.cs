using MediatR;
using ProjektIznynierski.Application.Dtos;

namespace ProjektIznynierski.Application.Queries.CurrencyRateHistory.GetCurrencyRateHistoryById
{
    public record GetCurrencyRateHistoryByIdQuery(int Id) : IRequest<CurrencyRateHistoryDto>;
}