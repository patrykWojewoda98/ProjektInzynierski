using MediatR;
using ProjektIznynierski.Application.Dtos;
using ProjektIznynierski.Domain.Abstractions;

namespace ProjektIznynierski.Application.Queries.CurrencyRateHistory.GetCurrencyRateHistoryByPairId
{
    internal class GetCurrencyRateHistoryByPairIdQueryHandler: IRequestHandler<GetCurrencyRateHistoryByPairIdQuery,List<CurrencyRateHistoryDto>>
    {
        private readonly ICurrencyRateHistoryRepository _rateHistoryRepository;

        public GetCurrencyRateHistoryByPairIdQueryHandler(ICurrencyRateHistoryRepository rateHistoryRepository)
        {
            _rateHistoryRepository = rateHistoryRepository;
        }

        public async Task<List<CurrencyRateHistoryDto>> Handle(GetCurrencyRateHistoryByPairIdQuery request,CancellationToken cancellationToken)
        {
            var history = await _rateHistoryRepository.GetHistoryAsync(request.currencyPairId);

            if (!history.Any())
            {
                throw new Exception(
                    $"No rate history found for currency pair ID {request.currencyPairId}."
                );
            }

            return history.Select(h => new CurrencyRateHistoryDto
            {
                Id = h.Id,
                CurrencyPairId = h.CurrencyPairId,
                Date = h.Date,
                CloseRate = h.CloseRate
            }).ToList();
        }
    }
}
