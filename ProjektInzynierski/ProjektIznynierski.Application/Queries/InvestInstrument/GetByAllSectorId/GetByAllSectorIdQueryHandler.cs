using MediatR;
using ProjektIznynierski.Application.Dtos;
using ProjektIznynierski.Application.Queries.InvestInstrument.GetInvestInstrumentById;
using ProjektIznynierski.Domain.Abstractions;

namespace ProjektIznynierski.Application.Queries.InvestInstrument.GetAllInvestInstruments
{
    public class GetByAllSectorIdQueryHandler : IRequestHandler<GetByAllSectorIdQuery, List<InvestInstrumentDto>>
    {
        private readonly IInvestInstrumentRepository _investInstrumentRepository;

        public GetByAllSectorIdQueryHandler(IInvestInstrumentRepository investInstrumentRepository)
        {
            _investInstrumentRepository = investInstrumentRepository;
        }

        public async Task<List<InvestInstrumentDto>> Handle(GetByAllSectorIdQuery request, CancellationToken cancellationToken)
        {
            var investInstruments = await _investInstrumentRepository.GetBySectorIdAsync(request.id, cancellationToken);

            return investInstruments.Select(i => new InvestInstrumentDto
            {
                Id = i.Id,
                Name = i.Name,
                Ticker = i.Ticker,
                InvestmentTypeId = i.InvestmentTypeId,
                Description = i.Description,
                MarketDataDate = i.MarketDataDate,
                SectorId = i.SectorId,
                RegionId = i.RegionId,
                CountryId = i.CountryId,
                CurrencyId = i.CurrencyId,
                FinancialMetricId = i.FinancialMetricId
            }).ToList();
        }
    }
}
