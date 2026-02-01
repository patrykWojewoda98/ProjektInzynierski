using MediatR;
using ProjektIznynierski.Application.Dtos;
using ProjektIznynierski.Domain.Abstractions;

namespace ProjektIznynierski.Application.Queries.CurrencyRateHistory.GetCurrencyRateHistoryById
{
    internal class GetCurrencyRateHistoryByIdQueryHandler : IRequestHandler<GetCurrencyRateHistoryByIdQuery, CurrencyRateHistoryDto>
    {
        private readonly ICurrencyRateHistoryRepository _rateHistoryRepository;

        public GetCurrencyRateHistoryByIdQueryHandler(ICurrencyRateHistoryRepository rateHistoryRepository)
        {
            _rateHistoryRepository = rateHistoryRepository;
        }

        public async Task<CurrencyRateHistoryDto> Handle(GetCurrencyRateHistoryByIdQuery request,CancellationToken cancellationToken)
        {
            var entity = await _rateHistoryRepository.GetByIdAsync(request.Id);

            if (entity is null)
            {
                throw new Exception(
                    $"Currency rate history with ID {request.Id} not found."
                );
            }

            return new CurrencyRateHistoryDto
            {
                Id = entity.Id,
                CurrencyPairId = entity.CurrencyPairId,
                Date = entity.Date,
                CloseRate = entity.CloseRate,
                OpenRate = entity.OpenRate,
                HighRate = entity.HighRate,
                LowRate = entity.LowRate
            };
        }
    }
}