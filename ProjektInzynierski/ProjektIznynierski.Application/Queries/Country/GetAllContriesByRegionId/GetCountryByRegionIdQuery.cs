using MediatR;
using ProjektIznynierski.Application.Dtos;

namespace ProjektIznynierski.Application.Queries.Country.GetAllContriesByRegionId
{
    public record GetCountryByRegionIdQuery(int regionID) : IRequest<List<CountryDto>>
    {
    }
}
