using MediatR;
using ProjektIznynierski.Application.Dtos;

namespace ProjektIznynierski.Application.Commands.Country.CreateCountry
{
    public class CreateCountryCommand : IRequest<CountryDto>
    {
        public string Name { get; set; }
        public string IsoCode { get; set; }
        public int RegionId { get; set; }
        public int CurrencyId { get; set; }
        public int CountryRisk { get; set; }
    }
}
