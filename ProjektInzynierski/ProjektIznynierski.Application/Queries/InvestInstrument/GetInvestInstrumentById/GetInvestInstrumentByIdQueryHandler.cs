using MediatR;
using ProjektIznynierski.Application.Dtos;
using ProjektIznynierski.Domain.Abstractions;

namespace ProjektIznynierski.Application.Queries.InvestInstrument.GetInvestInstrumentById
{
    internal class GetInvestInstrumentByIdQueryHandler : IRequestHandler<GetInvestInstrumentByIdQuery, InvestInstrumentDto>
    {
        private readonly IInvestInstrumentRepository _repository;
        public GetInvestInstrumentByIdQueryHandler(IInvestInstrumentRepository repository)
        {
            _repository = repository;
        }

        public async Task<InvestInstrumentDto> Handle(GetInvestInstrumentByIdQuery request, CancellationToken cancellationToken)
        {
            var entity = await _repository.GetByIdAsync(request.id, cancellationToken);
            if (entity is null)
            {
                throw new Exception($"InvestInstrument with id {request.id} not found.");
            }

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
