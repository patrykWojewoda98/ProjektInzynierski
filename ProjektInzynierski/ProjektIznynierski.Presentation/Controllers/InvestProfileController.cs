using MediatR;
using Microsoft.AspNetCore.Mvc;
using ProjektIznynierski.Application.Commands.InvestProfile.CreateInvestProfile;
using ProjektIznynierski.Application.Commands.InvestProfile.DeleteInvestProfile;
using ProjektIznynierski.Application.Commands.InvestProfile.UpdateInvestProfile;
using ProjektIznynierski.Application.Dtos;
using ProjektIznynierski.Application.Queries.InvestProfile.GetAllInvestProfiles;
using ProjektIznynierski.Application.Queries.InvestProfile.GetInvestProfileById;
using Swashbuckle.AspNetCore.Annotations;
using System.Net;

namespace ProjektIznynierski.Presentation.Controllers
{
    [Route("api/[controller]")]
    public class InvestProfileController : BaseController
    {
        public InvestProfileController(IMediator mediator) : base(mediator)
        {
        }

        [HttpGet]
        [SwaggerOperation(Summary = "Get Investment Profiles", Description = "Retrieves a list of all investment profiles.")]
        [ProducesResponseType(typeof(IEnumerable<InvestProfileDto>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> Get()
        {
            var result = await _mediator.Send(new GetAllInvestProfilesQuery());
            return Ok(result);
        }

        [HttpGet("{id}")]
        [SwaggerOperation(Summary = "Get Investment Profile by ID", Description = "Retrieves a specific investment profile by its ID.")]
        [ProducesResponseType(typeof(InvestProfileDto), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _mediator.Send(new GetInvestProfileByIdQuery(id));
            return Ok(result);
        }

        [HttpPost]
        [SwaggerOperation(Summary = "Create a new Investment Profile", Description = "Creates a new Investment Profile with the provided details.")]
        [ProducesResponseType(typeof(ClientDto), (int)HttpStatusCode.Created)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> Create([FromBody] CreateInvestProfileCommand command)
        {
            var result = await _mediator.Send(command);
            return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
        }

        [HttpPut("{id}")]
        [SwaggerOperation(Summary = "Update an existing Investment Profile", Description = "Updates an existing Investment Profile with the provided details.")]
        [ProducesResponseType(typeof(ClientDto), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateInvestProfileCommand command)
        {
            if (id != command.Id)
            {
                return BadRequest("ID in the URL does not match the ID in the request body.");
            }

            var result = await _mediator.Send(command);
            return Ok(result);
        }

        [HttpDelete("{id}")]
        [SwaggerOperation(Summary = "Delete a client", Description = "Deletes a specific Investment Profile by their ID.")]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> Delete(int id)
        {
            await _mediator.Send(new DeleteInvestProfileCommand { Id = id });
            return NoContent();
        }
    }
}
