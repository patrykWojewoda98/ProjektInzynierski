using MediatR;
using Microsoft.AspNetCore.Mvc;
using ProjektIznynierski.Application.Commands.Client.AddClient;
using ProjektIznynierski.Application.Commands.Client.DeleteClient;
using ProjektIznynierski.Application.Commands.Client.UpdateClient;
using ProjektIznynierski.Application.Commands.Sector.CreateSector;
using ProjektIznynierski.Application.Commands.Sector.DeleteSector;
using ProjektIznynierski.Application.Commands.Sector.UpdateSector;
using ProjektIznynierski.Application.Dtos;
using ProjektIznynierski.Application.Queries.Sector.GetAllSectrors;
using ProjektIznynierski.Application.Queries.Sector.GetSectorById;
using Swashbuckle.AspNetCore.Annotations;
using System.Net;

namespace ProjektIznynierski.Presentation.Controllers
{
    [Route("api/[controller]")]
    public class SectorController : BaseController
    {
        public SectorController(IMediator mediator) : base(mediator)
        {
        }

        [HttpGet]
        [SwaggerOperation(Summary = "Get Sectors", Description = "Retrieves a list of all sectors.")]
        [ProducesResponseType(typeof(IEnumerable<SectorDto>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> Get()
        {
            var result = await _mediator.Send(new GetAllSectorsQuery());
            return Ok(result);
        }

        [HttpGet("{id}")]
        [SwaggerOperation(Summary = "Get Sector by ID", Description = "Retrieves a specific sector by its ID.")]
        [ProducesResponseType(typeof(SectorDto), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _mediator.Send(new GetSectorByIdQuery(id));
            return Ok(result);
        }

        [HttpPost]
        [SwaggerOperation(Summary = "Create a new Sector", Description = "Creates a new Sector with the provided details.")]
        [ProducesResponseType(typeof(ClientDto), (int)HttpStatusCode.Created)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> Create([FromBody] CreateSectorCommand command)
        {
            var result = await _mediator.Send(command);
            return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
        }

        [HttpPut("{id}")]
        [SwaggerOperation(Summary = "Update an existing Sector", Description = "Updates an existing Sector with the provided details.")]
        [ProducesResponseType(typeof(ClientDto), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateSectorCommand command)
        {
            if (id != command.Id)
            {
                return BadRequest("ID in the URL does not match the ID in the request body.");
            }

            var result = await _mediator.Send(command);
            return Ok(result);
        }

        [HttpDelete("{id}")]
        [SwaggerOperation(Summary = "Delete a Sector", Description = "Deletes a specific Sector by their ID.")]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> Delete(int id)
        {
            await _mediator.Send(new DeleteSectorCommand { Id = id });
            return NoContent();
        }
    }
}
