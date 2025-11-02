using MediatR;
using ProjektIznynierski.Application.Dtos;
using ProjektIznynierski.Domain.Abstractions;

namespace ProjektIznynierski.Application.Commands.MarketData.UpdateMarketData
{
    internal class UpdateMarketDataCommandHandler : IRequestHandler<UpdateMarketDataCommand, MarketDataDto>
    {
        private readonly IMarketDataRepository _repository;
        private readonly IUnitOfWork _unitOfWork;
        public UpdateMarketDataCommandHandler(IMarketDataRepository repository, IUnitOfWork unitOfWork)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
        }

        public async Task<MarketDataDto> Handle(UpdateMarketDataCommand request, CancellationToken cancellationToken)
        {
            var entity = await _repository.GetByIdAsync(request.Id, cancellationToken);
            if (entity is null)
            {
                throw new Exception($"MarketData with id {request.Id} not found.");
            }

            entity.InvestInstrumentId = request.InvestInstrumentId;
            entity.Date = request.Date;
            entity.OpenPrice = request.OpenPrice;
            entity.ClosePrice = request.ClosePrice;
            entity.HighPrice = request.HighPrice;
            entity.LowPrice = request.LowPrice;
            entity.Volume = request.Volume;

            _repository.Update(entity);
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
