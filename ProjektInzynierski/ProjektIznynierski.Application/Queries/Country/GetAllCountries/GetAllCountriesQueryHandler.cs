using System.Collections.Generic;
using System.Linq;
using MediatR;
using ProjektIznynierski.Application.Dtos;
using ProjektIznynierski.Domain.Abstractions;

namespace ProjektIznynierski.Application.Queries.Country.GetAllCountries
{
    internal class GetAllCountriesQueryHandler : IRequestHandler<GetAllCountriesQuery, List<CountryDto>>
    {
        private readonly ICountryRepository _countryRepository;
        public GetAllCountriesQueryHandler(ICountryRepository countryRepository)
        {
            _countryRepository = countryRepository;
        }

        public async Task<List<CountryDto>> Handle(GetAllCountriesQuery request, CancellationToken cancellationToken)
        {
            var countries = await _countryRepository.GetAllAsync();
            return countries.Select(country => new CountryDto
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
