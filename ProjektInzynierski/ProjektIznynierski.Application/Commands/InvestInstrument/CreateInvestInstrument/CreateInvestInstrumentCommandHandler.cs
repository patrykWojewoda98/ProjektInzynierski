using MediatR;
using ProjektIznynierski.Application.Dtos;
using ProjektIznynierski.Domain.Abstractions;
using ProjektIznynierski.Domain.Enums;

namespace ProjektIznynierski.Application.Commands.InvestInstrument.CreateInvestInstrument
{
    internal class CreateInvestInstrumentCommandHandler : IRequestHandler<CreateInvestInstrumentCommand, InvestInstrumentDto>
    {
        private readonly IInvestInstrumentRepository _repository;
        private readonly IUnitOfWork _unitOfWork;
        public CreateInvestInstrumentCommandHandler(IInvestInstrumentRepository repository, IUnitOfWork unitOfWork)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
        }

        public async Task<InvestInstrumentDto> Handle(CreateInvestInstrumentCommand request, CancellationToken cancellationToken)
        {
            var entity = new Domain.Entities.InvestInstrument
            {
                Name = request.Name,
                Ticker = request.Ticker,
                Type = (InvestmentType)request.Type,
                Description = request.Description,
                MarketDataDate = request.MarketDataDate,
                SectorId = request.SectorId,
                RegionId = request.RegionId,
                CountryId = request.CountryId,
                CurrencyId = request.CurrencyId,
                FinancialMetricId = request.FinancialMetricId
            };
            _repository.Add(entity);
            await _unitOfWork.SaveChangesAsync();

            return new InvestInstrumentDto
            {
                Id = entity.Id,
                Name = entity.Name,
                Ticker = entity.Ticker,
                Type = (int)entity.Type,
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
