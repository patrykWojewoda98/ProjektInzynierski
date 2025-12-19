using MediatR;
using Microsoft.AspNetCore.Mvc;
using ProjektIznynierski.Application.Commands.RiskLevel.UpdateRiskLevel;
using ProjektIznynierski.Application.Commands.RiskLevel.DeleteRiskLevel;
using ProjektIznynierski.Application.Dtos;
using Swashbuckle.AspNetCore.Annotations;
using System.Net;
using ProjektIznynierski.Application.Commands.Country.CreateCountry;
using ProjektIznynierski.Application.Queries.InvestProfile.GetAllInvestProfiles;
using ProjektIznynierski.Application.Queries.InvestProfile.GetInvestProfileById;

namespace ProjektIznynierski.Presentation.Controllers
{
    [Route("api/[controller]")]
    public class RiskLevelController : BaseController
    {
        public RiskLevelController(IMediator mediator) : base(mediator)
        {
        }

        [HttpGet]
        [SwaggerOperation(
            Summary = "Get Risk Levels",
            Description = "Retrieves a list of all risk levels.")]
        [ProducesResponseType(typeof(IEnumerable<RiskLevelDto>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> Get()
        {
            var result = await _mediator.Send(new GetAllRiskLevelsQuery());
            return Ok(result);
        }

        [HttpGet("{id}")]
        [SwaggerOperation(
            Summary = "Get Risk Level by ID",
            Description = "Retrieves a specific risk level by its ID.")]
        [ProducesResponseType(typeof(RiskLevelDto), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _mediator.Send(new GetRiskLevelByIdQuery(id));
            return Ok(result);
        }

        [HttpPost]
        [SwaggerOperation(
            Summary = "Create a new risk level",
            Description = "Creates a new risk level with the provided details.")]
        [ProducesResponseType(typeof(RiskLevelDto), (int)HttpStatusCode.Created)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> Create([FromBody] CreateRiskLevelCommand command)
        {
            var result = await _mediator.Send(command);
            return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
        }

        [HttpPut("{id}")]
        [SwaggerOperation(
            Summary = "Update an existing risk level",
            Description = "Updates an existing risk level with the provided details.")]
        [ProducesResponseType(typeof(RiskLevelDto), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateRiskLevelCommand command)
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
            Summary = "Delete a risk level",
            Description = "Deletes a specific risk level by its ID.")]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> Delete(int id)
        {
            await _mediator.Send(new DeleteRiskLevelCommand { Id = id });
            return NoContent();
        }
    }
}
