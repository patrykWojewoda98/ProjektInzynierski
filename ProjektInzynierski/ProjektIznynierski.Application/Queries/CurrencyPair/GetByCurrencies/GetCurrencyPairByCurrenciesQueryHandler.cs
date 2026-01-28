using MediatR;
using ProjektIznynierski.Application.Dtos;
using ProjektIznynierski.Domain.Abstractions;

namespace ProjektIznynierski.Application.Queries.CurrencyPair.GetByCurrencies
{
    internal class GetCurrencyPairByCurrenciesQueryHandler: IRequestHandler<GetCurrencyPairByCurrenciesQuery,CurrencyPairDto>
    {
        private readonly ICurrencyPairRepository _currencyPairRepository;

        public GetCurrencyPairByCurrenciesQueryHandler(ICurrencyPairRepository currencyPairRepository)
        {
            _currencyPairRepository = currencyPairRepository;
        }

        public async Task<CurrencyPairDto> Handle(GetCurrencyPairByCurrenciesQuery request,CancellationToken cancellationToken)
        {
            var pair = await _currencyPairRepository.GetByCurrenciesAsync(request.BaseCurrencyId,request.QuoteCurrencyId);

            if (pair == null)
            {
                throw new Exception( $"Currency pair {request.BaseCurrencyId}/{request.QuoteCurrencyId} not found.");
            }

            return new CurrencyPairDto
            {
                Id = pair.Id,
                BaseCurrencyId = pair.BaseCurrencyId,
                QuoteCurrencyId = pair.QuoteCurrencyId,
                Symbol = pair.Symbol
            };
        }
    }
}
