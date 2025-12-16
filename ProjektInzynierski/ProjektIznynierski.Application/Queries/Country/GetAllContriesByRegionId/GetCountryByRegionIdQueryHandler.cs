using MediatR;
using ProjektIznynierski.Application.Dtos;
using ProjektIznynierski.Domain.Abstractions;

namespace ProjektIznynierski.Application.Queries.Country.GetAllContriesByRegionId
{
    internal class GetCountryByRegionIdQueryHandler : IRequestHandler<GetCountryByRegionIdQuery, List<CountryDto>>
    {
        private readonly ICountryRepository _countryRepository;
        public GetCountryByRegionIdQueryHandler(ICountryRepository countryRepository)
        {
            _countryRepository = countryRepository;
        }

        public async Task<List<CountryDto>> Handle(GetCountryByRegionIdQuery request, CancellationToken cancellationToken)
        {
            var countries = await _countryRepository.GetAllAsync(cancellationToken);
            var filteredCountries = countries.Where(c => c.RegionId == request.regionID).ToList();

            if (!filteredCountries.Any())
            {
                throw new Exception($"No countries found for region ID {request.regionID}.");
            }

            return filteredCountries.Select(country => new CountryDto
            {
                Id = country.Id,
                Name = country.Name,
                IsoCode = country.IsoCode,
                RegionId = country.RegionId,
                CurrencyId = country.CurrencyId,
                CountryRiskLevelId = country.CountryRiskLevelId
            }).ToList();
        }
    }
}
