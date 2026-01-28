using MediatR;
using ProjektIznynierski.Application.Dtos;

namespace ProjektIznynierski.Application.Queries.CurrencyPair.GetByCurrencies
{
    public record GetCurrencyPairByCurrenciesQuery(int BaseCurrencyId,int QuoteCurrencyId) : IRequest<CurrencyPairDto>
    {
    }
}
