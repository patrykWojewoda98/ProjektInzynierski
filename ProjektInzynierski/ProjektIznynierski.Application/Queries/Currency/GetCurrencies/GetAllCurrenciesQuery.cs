using MediatR;
using ProjektIznynierski.Application.Dtos;

namespace ProjektIznynierski.Application.Queries.Currency.GetCurrencies
{
    public record GetAllCurrenciesQuery : IRequest<List<CurrencyDto>>
    {

    }
}
