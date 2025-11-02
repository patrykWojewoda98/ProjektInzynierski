using MediatR;
using ProjektIznynierski.Application.Dtos;
using ProjektIznynierski.Domain.Abstractions;

namespace ProjektIznynierski.Application.Queries.MarketData.GetMarketDataById
{
    internal class GetMarketDataByIdQueryHandler : IRequestHandler<GetMarketDataByIdQuery, MarketDataDto>
    {
        private readonly IMarketDataRepository _repository;
        public GetMarketDataByIdQueryHandler(IMarketDataRepository repository)
        {
            _repository = repository;
        }

        public async Task<MarketDataDto> Handle(GetMarketDataByIdQuery request, CancellationToken cancellationToken)
        {
            var entity = await _repository.GetByIdAsync(request.id, cancellationToken);
            if (entity is null)
            {
                throw new Exception($"MarketData with id {request.id} not found.");
            }

            return new MarketDataDto
            {
                Id = entity.Id,
                InvestInstrumentId = entity.InvestInstrumentId,
                Date = entity.Date,
                OpenPrice = entity.OpenPrice,
                ClosePrice = entity.ClosePrice,
                HighPrice = entity.HighPrice,
                LowPrice = entity.LowPrice,
                Volume = entity.Volume,
                DailyChange = entity.DailyChange,
                DailyChangePercent = entity.DailyChangePercent
            };
        }
    }
}
