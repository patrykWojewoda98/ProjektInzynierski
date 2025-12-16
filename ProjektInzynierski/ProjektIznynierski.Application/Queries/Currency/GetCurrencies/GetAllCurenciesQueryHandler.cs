using MediatR;
using ProjektIznynierski.Application.Dtos;
using ProjektIznynierski.Domain.Abstractions;

namespace ProjektIznynierski.Application.Queries.Currency.GetCurrencies
{
    public class GetAllCurenciesQueryHandler : IRequestHandler<GetAllCurenciesQuery, List<CurrencyDto>>
    {
        private readonly ICurrencyRepository _currencyRepository;
        
        public GetAllCurenciesQueryHandler(ICurrencyRepository currencyRepository)
        {
            _currencyRepository = currencyRepository;
        }

        public async Task<List<CurrencyDto>> Handle(GetAllCurenciesQuery request, CancellationToken cancellationToken)
        {
            var currencies = await _currencyRepository.GetAllAsync(cancellationToken);
            return currencies.Select(currency => new CurrencyDto
            {
                Id = currency.Id,
                Name = currency.Name,
                CurrencyRiskLevelId = currency.CurrencyRiskLevelId
            }).ToList();
        }
    }
}
