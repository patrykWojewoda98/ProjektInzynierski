using MediatR;
using Microsoft.AspNetCore.Mvc;
using ProjektIznynierski.Application.Commands.InvestHorizon.AddInvestHorizon;
using ProjektIznynierski.Application.Commands.InvestHorizon.DeleteInvestHorizon;
using ProjektIznynierski.Application.Commands.InvestHorizon.UpdateInvestHorizon;
using ProjektIznynierski.Application.Dtos;
using ProjektIznynierski.Application.Queries.InvestHorizon.GetAllInvestHorizons;
using ProjektIznynierski.Application.Queries.InvestHorizon.GetInvestHorizonByID;
using Swashbuckle.AspNetCore.Annotations;
using System.Net;

namespace ProjektIznynierski.Presentation.Controllers
{
    [Route("api/[controller]")]
    public class InvestHorizonController : BaseController
    {
        public InvestHorizonController(IMediator mediator) : base(mediator)
        {
        }

        [HttpGet]
        [SwaggerOperation(Summary = "Get InvestHorizon", Description = "Retrieves a list of all InvestHorizons.")]
        [ProducesResponseType(typeof(IEnumerable<InvestHorizonDto>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> Get()
        {
            var result = await _mediator.Send(new GetAllInvestHorizonsQuery());
            return Ok(result);
        }

        [HttpGet("{id}")]
        [SwaggerOperation(Summary = "Get InvestHorizon by ID", Description = "Retrieves a specific InvestHorizon by their ID.")]
        [ProducesResponseType(typeof(InvestHorizonDto), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _mediator.Send(new GetInvestHorizonByIdQuery(id));
            return Ok(result);
        }

        [HttpPost]
        [SwaggerOperation(Summary = "Create a new InvestHorizon", Description = "Creates a new InvestHorizon with the provided details.")]
        [ProducesResponseType(typeof(InvestHorizonDto), (int)HttpStatusCode.Created)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> Create([FromBody] AddInvestHorizonCommand command)
        {
            var result = await _mediator.Send(command);
            return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
        }

        [HttpPut("{id}")]
        [SwaggerOperation(Summary = "Update an existing InvestHorizon", Description = "Updates an existing InvestHorizon with the provided details.")]
        [ProducesResponseType(typeof(InvestHorizonDto), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateInvestHorizonCommand command)
        {
            if (id != command.Id)
            {
                return BadRequest("ID in the URL does not match the ID in the request body.");
            }

            var result = await _mediator.Send(command);
            return Ok(result);
        }

        [HttpDelete("{id}")]
        [SwaggerOperation(Summary = "Delete a InvestHorizon", Description = "Deletes a specific InvestHorizon by their ID.")]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> Delete(int id)
        {
            await _mediator.Send(new DeleteInvestHorizonCommand { Id = id });
            return NoContent();
        }
    }
}
