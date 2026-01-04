using MediatR;
using Microsoft.AspNetCore.Mvc;
using ProjektIznynierski.Application.Commands.RegionCode.CreateRegionCode;
using ProjektIznynierski.Application.Commands.RegionCode.DeleteRegionCode;
using ProjektIznynierski.Application.Commands.RegionCode.UpdateRegionCode;
using ProjektIznynierski.Application.Dtos;
using ProjektIznynierski.Application.Queries.RegionCode.GetAllRegionCodes;
using ProjektIznynierski.Application.Queries.RegionCode.GetRegionCodeById;
using Swashbuckle.AspNetCore.Annotations;
using System.Net;

namespace ProjektIznynierski.Presentation.Controllers
{
    [Route("api/[controller]")]
    public class RegionCodeController : BaseController
    {
        public RegionCodeController(IMediator mediator) : base(mediator)
        {
        }

        [HttpGet]
        [SwaggerOperation(
            Summary = "Get Region Codes",
            Description = "Retrieves a list of all region codes.")]
        [ProducesResponseType(typeof(IEnumerable<RegionCodeDto>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> Get()
        {
            var result = await _mediator.Send(new GetAllRegionCodesQuery());
            return Ok(result);
        }

        [HttpGet("{id}")]
        [SwaggerOperation(
            Summary = "Get Region Code by ID",
            Description = "Retrieves a specific region code by its ID.")]
        [ProducesResponseType(typeof(RegionCodeDto), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _mediator.Send(new GetRegionCodeByIdQuery(id));
            return Ok(result);
        }

        [HttpPost]
        [SwaggerOperation(
            Summary = "Create a new region code",
            Description = "Creates a new region code with the provided details.")]
        [ProducesResponseType(typeof(RegionCodeDto), (int)HttpStatusCode.Created)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> Create(
            [FromBody] CreateRegionCodeCommand command)
        {
            var result = await _mediator.Send(command);

            return CreatedAtAction(
                nameof(GetById),
                new { id = result.Id },
                result);
        }

        [HttpPut("{id}")]
        [SwaggerOperation(
            Summary = "Update an existing region code",
            Description = "Updates an existing region code with the provided details.")]
        [ProducesResponseType(typeof(RegionCodeDto), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> Update(
            int id,
            [FromBody] UpdateRegionCodeCommand command)
        {
            if (id != command.Id)
            {
                return BadRequest(
                    "ID in the URL does not match the ID in the request body.");
            }

            var result = await _mediator.Send(command);
            return Ok(result);
        }

        [HttpDelete("{id}")]
        [SwaggerOperation(
            Summary = "Delete a region code",
            Description = "Deletes a specific region code by its ID.")]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> Delete(int id)
        {
            await _mediator.Send(new DeleteRegionCodeCommand { Id = id });
            return NoContent();
        }
    }
}
