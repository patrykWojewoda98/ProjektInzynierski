using MediatR;
using Microsoft.AspNetCore.Mvc;
using ProjektIznynierski.Application.Dtos;
using ProjektIznynierski.Application.Queries.AIAnalysisResult.GetAllAIAnalysisResults;
using ProjektIznynierski.Application.Queries.AIAnalysisResult.GetAIAnalysisResultById;
using Swashbuckle.AspNetCore.Annotations;
using System.Net;
using ProjektIznynierski.Application.Commands.Client.AddClient;
using ProjektIznynierski.Application.Commands.Client.DeleteClient;
using ProjektIznynierski.Application.Commands.Client.UpdateClient;
using ProjektIznynierski.Application.Commands.AIAnalysisResult.CreateAIAnalysisResult;
using ProjektIznynierski.Application.Commands.AIAnalysisResult.UpdateAIAnalysisResult;
using ProjektIznynierski.Application.Commands.AIAnalysisResult.DeleteAIAnalysisResult;

namespace ProjektIznynierski.Presentation.Controllers
{
    [Route("api/[controller]")]
    public class AIAnalysisResultController : BaseController
    {
        public AIAnalysisResultController(IMediator mediator) : base(mediator)
        {
        }

        [HttpGet]
        [SwaggerOperation(Summary = "Get AI Analysis Results", Description = "Retrieves a list of all AI analysis results.")]
        [ProducesResponseType(typeof(IEnumerable<AIAnalysisResultDto>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> Get()
        {
            var result = await _mediator.Send(new GetAllAIAnalysisResultsQuery());
            return Ok(result);
        }

        [HttpGet("{id}")]
        [SwaggerOperation(Summary = "Get AI Analysis Result by ID", Description = "Retrieves a specific AI analysis result by its ID.")]
        [ProducesResponseType(typeof(AIAnalysisResultDto), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _mediator.Send(new GetAIAnalysisResultByIdQuery(id));
            return Ok(result);
        }

        [HttpPost]
        [SwaggerOperation(Summary = "Create a new Analysis Result", Description = "Creates a new Analysis Result with the provided details.")]
        [ProducesResponseType(typeof(AIAnalysisResultDto), (int)HttpStatusCode.Created)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> Create([FromBody] CreateAIAnalysisResultCommand command)
        {
            var result = await _mediator.Send(command);
            return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
        }

        [HttpPut("{id}")]
        [SwaggerOperation(Summary = "Update an existing Analysis Result", Description = "Updates an existing Analysis Result with the provided details.")]
        [ProducesResponseType(typeof(AIAnalysisResultDto), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateAIAnalysisResultCommand command)
        {
            if (id != command.Id)
            {
                return BadRequest("ID in the URL does not match the ID in the request body.");
            }

            var result = await _mediator.Send(command);
            return Ok(result);
        }

        [HttpDelete("{id}")]
        [SwaggerOperation(Summary = "Delete a Analysis Result", Description = "Deletes a specific Analysis Result by their ID.")]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> Delete(int id)
        {
            await _mediator.Send(new DeleteAIAnalysisResultCommand { Id = id });
            return NoContent();
        }
    }
}
