using MediatR;
using ProjektIznynierski.Application.Dtos;

namespace ProjektIznynierski.Application.Queries.Country.GetCountryById
{
    public record GetCountryByIdQuery(int id) : IRequest<CountryDto>
    {
    }
}
