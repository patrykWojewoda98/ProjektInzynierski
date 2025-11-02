using MediatR;
using ProjektIznynierski.Application.Dtos;
using ProjektIznynierski.Domain.Abstractions;

namespace ProjektIznynierski.Application.Commands.MarketData.CreateMarketData
{
    internal class CreateMarketDataCommandHandler : IRequestHandler<CreateMarketDataCommand, MarketDataDto>
    {
        private readonly IMarketDataRepository _repository;
        private readonly IUnitOfWork _unitOfWork;
        public CreateMarketDataCommandHandler(IMarketDataRepository repository, IUnitOfWork unitOfWork)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
        }

        public async Task<MarketDataDto> Handle(CreateMarketDataCommand request, CancellationToken cancellationToken)
        {
            var entity = new Domain.Entities.MarketData
            {
                InvestInstrumentId = request.InvestInstrumentId,
                Date = request.Date,
                OpenPrice = request.OpenPrice,
                ClosePrice = request.ClosePrice,
                HighPrice = request.HighPrice,
                LowPrice = request.LowPrice,
                Volume = request.Volume
            };
            _repository.Add(entity);
            await _unitOfWork.SaveChangesAsync();

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
