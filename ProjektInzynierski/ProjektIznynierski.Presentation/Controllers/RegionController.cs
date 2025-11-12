using MediatR;
using Microsoft.AspNetCore.Mvc;
using ProjektIznynierski.Application.Commands.Region.CreateRegion;
using ProjektIznynierski.Application.Commands.Region.DeleteRegion;
using ProjektIznynierski.Application.Commands.Region.UpdateRegion;
using ProjektIznynierski.Application.Dtos;
using ProjektIznynierski.Application.Queries.Region.GetAllRegions;
using ProjektIznynierski.Application.Queries.Region.GetRegionById;
using Swashbuckle.AspNetCore.Annotations;
using System.Net;

namespace ProjektIznynierski.Presentation.Controllers
{
    [Route("api/[controller]")]
    public class RegionController : BaseController
    {
        public RegionController(IMediator mediator) : base(mediator)
        {
        }

        [HttpGet]
        [SwaggerOperation(Summary = "Get Regions", Description = "Retrieves a list of all regions.")]
        [ProducesResponseType(typeof(IEnumerable<RegionDto>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> Get()
        {
            var result = await _mediator.Send(new GetAllRegionsQuery());
            return Ok(result);
        }

        [HttpGet("{id}")]
        [SwaggerOperation(Summary = "Get Region by ID", Description = "Retrieves a specific region by its ID.")]
        [ProducesResponseType(typeof(RegionDto), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _mediator.Send(new GetRegionByIdQuery(id));
            return Ok(result);
        }

        [HttpPost]
        [SwaggerOperation(Summary = "Create a new region", Description = "Creates a new region with the provided details.")]
        [ProducesResponseType(typeof(RegionDto), (int)HttpStatusCode.Created)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> Create([FromBody] CreateRegionCommand command)
        {
            var result = await _mediator.Send(command);
            return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
        }

        [HttpPut("{id}")]
        [SwaggerOperation(Summary = "Update an existing region", Description = "Updates an existing region with the provided details.")]
        [ProducesResponseType(typeof(RegionDto), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateRegionCommand command)
        {
            if (id != command.Id)
            {
                return BadRequest("ID in the URL does not match the ID in the request body.");
            }

            var result = await _mediator.Send(command);
            return Ok(result);
        }

        [HttpDelete("{id}")]
        [SwaggerOperation(Summary = "Delete a region", Description = "Deletes a specific region by its ID.")]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> Delete(int id)
        {
            await _mediator.Send(new DeleteRegionCommand { Id = id });
            return NoContent();
        }
    }
}
