using MediatR;
using ProjektIznynierski.Application.Dtos;
using ProjektIznynierski.Domain.Abstractions;

namespace ProjektIznynierski.Application.Queries.CurrencyRateHistory.GetAllCurrencyRates
{
    internal class GetAllCurrencyRatesQueryHandler : IRequestHandler<GetAllCurrencyRatesQuery, List<CurrencyRateHistoryDto>>
    {
        private readonly ICurrencyRateHistoryRepository _rateHistoryRepository;

        public GetAllCurrencyRatesQueryHandler(ICurrencyRateHistoryRepository rateHistoryRepository)
        {
            _rateHistoryRepository = rateHistoryRepository;
        }

        public async Task<List<CurrencyRateHistoryDto>> Handle(
            GetAllCurrencyRatesQuery request,
            CancellationToken cancellationToken
        )
        {
            var rates = await _rateHistoryRepository.GetAllAsync(cancellationToken);

            if (!rates.Any())
            {
                throw new Exception("No currency rates found.");
            }

            return rates.Select(rate => new CurrencyRateHistoryDto
            {
                Id = rate.Id,
                CurrencyPairId = rate.CurrencyPairId,
                Date = rate.Date,
                CloseRate = rate.CloseRate,
                OpenRate = rate.OpenRate,
                HighRate = rate.HighRate,
                LowRate = rate.LowRate
            }).ToList();
        }
    }
}
