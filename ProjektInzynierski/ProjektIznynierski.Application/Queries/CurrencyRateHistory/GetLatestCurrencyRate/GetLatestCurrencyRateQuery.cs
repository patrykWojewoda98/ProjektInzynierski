using MediatR;
using ProjektIznynierski.Application.Dtos;

namespace ProjektIznynierski.Application.Queries.CurrencyRateHistory.GetLatestRate
{
    public record GetLatestCurrencyRateQuery(int currencyPairId) : IRequest<CurrencyRateHistoryDto>
    {
    }
}
