using MediatR;
using Microsoft.AspNetCore.Mvc;
using ProjektIznynierski.Application.Dtos;
using ProjektIznynierski.Application.Queries.Country.GetAllCountries;
using ProjektIznynierski.Application.Queries.Country.GetCountryById;
using Swashbuckle.AspNetCore.Annotations;
using System.Net;

namespace ProjektIznynierski.Presentation.Controllers
{
    [Route("api/[controller]")]
    public class CountryController : BaseController
    {
        public CountryController(IMediator mediator) : base(mediator)
        {
        }

        [HttpGet]
        [SwaggerOperation(Summary = "Get Countries", Description = "Retrieves a list of all countries.")]
        [ProducesResponseType(typeof(IEnumerable<CountryDto>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> Get()
        {
            var result = await _mediator.Send(new GetAllCountriesQuery());
            return Ok(result);
        }

        [HttpGet("{id}")]
        [SwaggerOperation(Summary = "Get Country by ID", Description = "Retrieves a specific country by its ID.")]
        [ProducesResponseType(typeof(CountryDto), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _mediator.Send(new GetCountryByIdQuery(id));
            return Ok(result);
        }
    }
}
