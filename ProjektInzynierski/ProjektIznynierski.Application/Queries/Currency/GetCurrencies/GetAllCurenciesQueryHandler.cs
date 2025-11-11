using ProjektIznynierski.Application.Dtos;
using ProjektIznynierski.Domain.Abstractions;

namespace ProjektIznynierski.Application.Queries.Currency.GetCurrencies
{
    internal class GetAllCurenciesQueryHandler
    {
        private readonly ICurrencyRepository _currencyRepository;
        public GetAllCurenciesQueryHandler(ICurrencyRepository currencyRepository)
        {
            _currencyRepository = currencyRepository;
        }

        public async Task<List<CurrencyDto>> Handle(GetAllCurenciesQuery, CancellationToken cancellationToken)
        {
            var currencies = await _currencyRepository.GetAllAsync();
            return currencies.Select(currency => new CurrencyDto
            {
                Id = currency.Id,
                Name = currency.Name,
                CurrencyRisk = (int)currency.CurrencyRisk
            }).ToList();
        }
    }
}
