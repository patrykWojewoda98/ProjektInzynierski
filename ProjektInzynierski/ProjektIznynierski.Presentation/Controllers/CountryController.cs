using MediatR;
using Microsoft.AspNetCore.Mvc;
using ProjektIznynierski.Application.Commands.Country.CreateCountry;
using ProjektIznynierski.Application.Commands.Country.DeleteCountry;
using ProjektIznynierski.Application.Commands.Country.UpdateCountry;
using ProjektIznynierski.Application.Dtos;
using ProjektIznynierski.Application.Queries.Country.GetAllContriesByRegionId;
using ProjektIznynierski.Application.Queries.Country.GetAllCountries;
using ProjektIznynierski.Application.Queries.Country.GetCountryById;
using ProjektIznynierski.Domain.Entities;
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
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _mediator.Send(new GetCountryByIdQuery(id));
            return Ok(result);
        }

        [HttpGet("region/{regionId}")]
        [SwaggerOperation(Summary = "Get Countryies by RegionID", Description = "Retrieves a specific country by its RegionID.")]
        [ProducesResponseType(typeof(CountryDto), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> GetByRegionId(int regionId)
        {
            var result = await _mediator.Send(new GetCountryByRegionIdQuery(regionId));
            return Ok(result);
        }

        [HttpPost]
        [SwaggerOperation(Summary = "Create a new country", Description = "Creates a new country with the provided details.")]
        [ProducesResponseType(typeof(CountryDto), (int)HttpStatusCode.Created)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> Create([FromBody] CreateCountryCommand command)
        {
            var result = await _mediator.Send(command);
            return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
        }

        [HttpPut("{id}")]
        [SwaggerOperation(Summary = "Update an existing country", Description = "Updates an existing country with the provided details.")]
        [ProducesResponseType(typeof(CountryDto), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateCountryCommand command)
        {
            if (id != command.Id)
            {
                return BadRequest("ID in the URL does not match the ID in the request body.");
            }

            var result = await _mediator.Send(command);
            return Ok(result);
        }

        [HttpDelete("{id}")]
        [SwaggerOperation(Summary = "Delete a country", Description = "Deletes a specific country by its ID.")]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> Delete(int id)
        {
            await _mediator.Send(new DeleteCountryCommand { Id = id });
            return NoContent();
        }
    }
}
