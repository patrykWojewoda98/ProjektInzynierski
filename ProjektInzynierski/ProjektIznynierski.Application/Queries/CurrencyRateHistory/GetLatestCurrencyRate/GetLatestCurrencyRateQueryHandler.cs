using MediatR;
using ProjektIznynierski.Application.Dtos;
using ProjektIznynierski.Domain.Abstractions;

namespace ProjektIznynierski.Application.Queries.CurrencyRateHistory.GetLatestRate
{
    internal class GetLatestCurrencyRateQueryHandler: IRequestHandler<GetLatestCurrencyRateQuery,CurrencyRateHistoryDto>
    {
        private readonly ICurrencyRateHistoryRepository _rateHistoryRepository;

        public GetLatestCurrencyRateQueryHandler(ICurrencyRateHistoryRepository rateHistoryRepository)
        {
            _rateHistoryRepository = rateHistoryRepository;
        }

        public async Task<CurrencyRateHistoryDto> Handle(GetLatestCurrencyRateQuery request,CancellationToken cancellationToken)
        {
            var latestRate =
                await _rateHistoryRepository.GetLatestRateAsync(
                    request.currencyPairId
                );

            if (latestRate == null)
            {
                throw new Exception(
                    $"No latest rate found for currency pair ID {request.currencyPairId}."
                );
            }

            return new CurrencyRateHistoryDto
            {
                Id = latestRate.Id,
                CurrencyPairId = latestRate.CurrencyPairId,
                Date = latestRate.Date,
                CloseRate = latestRate.CloseRate
            };
        }
    }
}
