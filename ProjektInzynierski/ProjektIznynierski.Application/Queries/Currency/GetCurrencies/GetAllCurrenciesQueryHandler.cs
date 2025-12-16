using MediatR;
using ProjektIznynierski.Application.Dtos;
using ProjektIznynierski.Domain.Abstractions;

namespace ProjektIznynierski.Application.Queries.Currency.GetCurrencies
{
    internal class GetAllCurrenciesQueryHandler : IRequestHandler<GetAllCurrenciesQuery, List<CurrencyDto>>
    {
        private readonly ICurrencyRepository _currencyRepository;

        public GetAllCurrenciesQueryHandler(ICurrencyRepository currencyRepository)
        {
            _currencyRepository = currencyRepository;
        }

        public async Task<List<CurrencyDto>> Handle(GetAllCurrenciesQuery request, CancellationToken cancellationToken)
        {
            var currencies = await _currencyRepository.GetAllAsync();

            return currencies.Select(currency => new CurrencyDto
            {
                Id = currency.Id,
                Name = currency.Name,
                CurrencyRiskLevelId = currency.CurrencyRiskLevelId
            }).ToList();
        }
    }
}
