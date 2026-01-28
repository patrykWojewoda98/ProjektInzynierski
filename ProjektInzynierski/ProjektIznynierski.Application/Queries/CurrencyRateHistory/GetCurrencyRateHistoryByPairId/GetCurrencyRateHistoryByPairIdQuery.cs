using MediatR;
using ProjektIznynierski.Application.Dtos;

namespace ProjektIznynierski.Application.Queries.CurrencyRateHistory.GetCurrencyRateHistoryByPairId
{
    public record GetCurrencyRateHistoryByPairIdQuery(int currencyPairId): IRequest<List<CurrencyRateHistoryDto>>
    {
    }
}