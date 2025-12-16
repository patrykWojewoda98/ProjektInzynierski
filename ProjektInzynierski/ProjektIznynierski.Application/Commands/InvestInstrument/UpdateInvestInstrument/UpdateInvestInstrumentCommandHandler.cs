using MediatR;
using ProjektIznynierski.Application.Dtos;
using ProjektIznynierski.Domain.Abstractions;
using ProjektIznynierski.Domain.Enums;

namespace ProjektIznynierski.Application.Commands.InvestInstrument.UpdateInvestInstrument
{
    internal class UpdateInvestInstrumentCommandHandler : IRequestHandler<UpdateInvestInstrumentCommand, InvestInstrumentDto>
    {
        private readonly IInvestInstrumentRepository _repository;
        private readonly IUnitOfWork _unitOfWork;
        public UpdateInvestInstrumentCommandHandler(IInvestInstrumentRepository repository, IUnitOfWork unitOfWork)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
        }

        public async Task<InvestInstrumentDto> Handle(UpdateInvestInstrumentCommand request, CancellationToken cancellationToken)
        {
            var entity = await _repository.GetByIdAsync(request.Id, cancellationToken);
            if (entity is null)
            {
                throw new Exception($"InvestInstrument with id {request.Id} not found.");
            }

            entity.Name = request.Name;
            entity.Ticker = request.Ticker;
            entity.InvestmentTypeId = request.InvestmentTypeId;
            entity.Description = request.Description;
            entity.MarketDataDate = request.MarketDataDate;
            entity.SectorId = request.SectorId;
            entity.RegionId = request.RegionId;
            entity.CountryId = request.CountryId;
            entity.CurrencyId = request.CurrencyId;
            entity.FinancialMetricId = request.FinancialMetricId;

            _repository.Update(entity);
            await _unitOfWork.SaveChangesAsync();

            return new InvestInstrumentDto
            {
                Id = entity.Id,
                Name = entity.Name,
                Ticker = entity.Ticker,
                InvestmentTypeId = entity.InvestmentTypeId,
                Description = entity.Description,
                MarketDataDate = entity.MarketDataDate,
                SectorId = entity.SectorId,
                RegionId = entity.RegionId,
                CountryId = entity.CountryId,
                CurrencyId = entity.CurrencyId,
                FinancialMetricId = entity.FinancialMetricId
            };
        }
    }
}
