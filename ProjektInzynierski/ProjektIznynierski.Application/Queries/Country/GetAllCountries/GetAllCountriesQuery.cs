using System.Collections.Generic;
using MediatR;
using ProjektIznynierski.Application.Dtos;

namespace ProjektIznynierski.Application.Queries.Country.GetAllCountries
{
    public record GetAllCountriesQuery() : IRequest<List<CountryDto>>
    {
    }
}
