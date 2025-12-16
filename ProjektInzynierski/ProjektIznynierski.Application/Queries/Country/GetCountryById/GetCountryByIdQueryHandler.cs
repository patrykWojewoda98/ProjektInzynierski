using MediatR;
using ProjektIznynierski.Application.Dtos;
using ProjektIznynierski.Domain.Abstractions;

namespace ProjektIznynierski.Application.Queries.Country.GetCountryById
{
    internal class GetCountryByIdQueryHandler : IRequestHandler<GetCountryByIdQuery, CountryDto>
    {
        private readonly ICountryRepository _countryRepository;
        public GetCountryByIdQueryHandler(ICountryRepository countryRepository)
        {
            _countryRepository = countryRepository;
        }

        public async Task<CountryDto> Handle(GetCountryByIdQuery request, CancellationToken cancellationToken)
        {
            var country = await _countryRepository.GetByIdAsync(request.id, cancellationToken);
            if (country is null)
            {
                throw new Exception($"Country with id {request.id} not found.");
            }

            return new CountryDto
            {
                Id = country.Id,
                Name = country.Name,
                IsoCode = country.IsoCode,
                RegionId = country.RegionId,
                CurrencyId = country.CurrencyId,
                CountryRiskLevelId = country.CountryRiskLevelId
            };
        }
    }
}
