using MediatR;
using ProjektIznynierski.Application.Dtos;
using ProjektIznynierski.Application.Queries.InvestInstrument.GetInvestInstrumentById;
using ProjektIznynierski.Domain.Abstractions;

namespace ProjektIznynierski.Application.Queries.InvestInstrument.GetAllInvestInstruments
{
    public class GetAllByRegionIdQueryHandler : IRequestHandler<GetAllByRegionIdQuery, List<InvestInstrumentDto>>
    {
        private readonly IInvestInstrumentRepository _investInstrumentRepository;

        public GetAllByRegionIdQueryHandler(IInvestInstrumentRepository investInstrumentRepository)
        {
            _investInstrumentRepository = investInstrumentRepository;
        }

        public async Task<List<InvestInstrumentDto>> Handle(GetAllByRegionIdQuery request, CancellationToken cancellationToken)
        {
            var investInstruments = await _investInstrumentRepository.GetByRegionIdAsync(request.id, cancellationToken);

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
                FinancialMetricId = i.FinancialMetricId,
                Isin = i.Isin
            }).ToList();
        }
    }
}
