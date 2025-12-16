using MediatR;
using ProjektIznynierski.Application.Dtos;

namespace ProjektIznynierski.Application.Commands.Country.UpdateCountry
{
    public class UpdateCountryCommand : IRequest<CountryDto>
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string IsoCode { get; set; }
        public int RegionId { get; set; }
        public int CurrencyId { get; set; }
        public int CountryRiskLevelId { get; set; }
    }
}
