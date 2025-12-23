using MediatR;
using Microsoft.AspNetCore.Mvc;
using ProjektIznynierski.Application.Dtos;
using Swashbuckle.AspNetCore.Annotations;
using System.Net;
using ProjektIznynierski.Application.Queries.InvestInstrument.GetAllByRegionId;
using ProjektIznynierski.Application.Commands.Country.CreateCountry;
using ProjektIznynierski.Application.Commands.RiskLevel.DeleteRiskLevel;
using ProjektIznynierski.Application.Commands.RiskLevel.UpdateRiskLevel;
using ProjektIznynierski.Application.Queries.InvestProfile.GetAllInvestProfiles;
using ProjektIznynierski.Application.Queries.InvestProfile.GetInvestProfileById;
using ProjektIznynierski.Application.Commands.Country.UpdateCountry;
using ProjektIznynierski.Application.Commands.InvestmentType.DeleteInvestmentType;
using ProjektIznynierski.Application.Queries.InvestHorizon.GetInvestHorizonByID;
using ProjektIznynierski.Application.Queries.InvestmentType.GetInvestmentTypeById;

namespace ProjektIznynierski.Presentation.Controllers
{
    [Route("api/[controller]")]
    public class InvestmentTypeController : BaseController
    {
        public InvestmentTypeController(IMediator mediator) : base(mediator)
        {
        }

        [HttpGet]
        [SwaggerOperation(
            Summary = "Get Investment Types",
            Description = "Retrieves a list of all Investment Types.")]
        [ProducesResponseType(typeof(IEnumerable<InvestmentTypeDto>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> Get()
        {
            var result = await _mediator.Send(new GetAllInvestmentTypesQuery());
            return Ok(result);
        }

        

        [HttpPost]
        [SwaggerOperation(
            Summary = "Create a new Investment Type",
            Description = "Creates a new Investment Type with the provided details.")]
        [ProducesResponseType(typeof(InvestmentTypeDto), (int)HttpStatusCode.Created)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> Create([FromBody] CreateInvestmentTypeCommand command)
        {
            var result = await _mediator.Send(command);
            return CreatedAtAction(nameof(GetInvestmentTypeByIdQuery), new { id = result.Id }, result);
        }

        [HttpPut("{id}")]
        [SwaggerOperation(
            Summary = "Update an existing Investment Type",
            Description = "Updates an existing Investment Type with the provided details.")]
        [ProducesResponseType(typeof(InvestmentTypeDto), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateInvestmentTypeCommand command)
        {
            if (id != command.Id)
            {
                return BadRequest("ID in the URL does not match the ID in the request body.");
            }

            var result = await _mediator.Send(command);
            return Ok(result);
        }

        [HttpDelete("{id}")]
        [SwaggerOperation(
            Summary = "Delete a Investment Type",
            Description = "Deletes a specific Investment Type by its ID.")]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> Delete(int id)
        {
            await _mediator.Send(new DeleteInvestmentTypeCommand { Id = id });
            return NoContent();
        }
    }
}

