using MediatR;
using Microsoft.AspNetCore.Mvc;
using ProjektIznynierski.Application.Dtos;
using ProjektIznynierski.Application.Queries.AIAnalysisResult.GetAllAIAnalysisResults;
using ProjektIznynierski.Application.Queries.AIAnalysisResult.GetAIAnalysisResultById;
using Swashbuckle.AspNetCore.Annotations;
using System.Net;

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
