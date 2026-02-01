using MediatR;
using ProjektIznynierski.Application.Dtos;

namespace ProjektIznynierski.Application.Queries.CurrencyRateHistory.GetAllCurrencyRates
{
    public record GetAllCurrencyRatesQuery() : IRequest<List<CurrencyRateHistoryDto>>
    {
    }
}
