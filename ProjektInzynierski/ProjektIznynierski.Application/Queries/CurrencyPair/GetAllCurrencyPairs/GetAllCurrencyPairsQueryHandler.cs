using MediatR;
using ProjektIznynierski.Application.Dtos;
using ProjektIznynierski.Domain.Abstractions;

namespace ProjektIznynierski.Application.Queries.CurrencyPair.GetAllCurrencyPairs
{
    internal class GetAllCurrencyPairsQueryHandler: IRequestHandler<GetAllCurrencyPairsQuery, List<CurrencyPairDto>>
    {
        private readonly ICurrencyPairRepository _currencyPairRepository;

        public GetAllCurrencyPairsQueryHandler(ICurrencyPairRepository currencyPairRepository)
        {
            _currencyPairRepository = currencyPairRepository;
        }

        public async Task<List<CurrencyPairDto>> Handle(
            GetAllCurrencyPairsQuery request,
            CancellationToken cancellationToken
        )
        {
            var pairs = await _currencyPairRepository.GetAllAsync(cancellationToken);

            return pairs.Select(cp => new CurrencyPairDto
            {
                Id = cp.Id,
                BaseCurrencyId = cp.BaseCurrencyId,
                QuoteCurrencyId = cp.QuoteCurrencyId,
                Symbol = cp.Symbol
            }).ToList();
        }
    }
}
