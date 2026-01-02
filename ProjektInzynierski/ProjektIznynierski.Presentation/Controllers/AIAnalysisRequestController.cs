using MediatR;
using Microsoft.AspNetCore.Mvc;
using ProjektIznynierski.Application.Dtos;
using ProjektIznynierski.Application.Queries.AIAnalysisRequest.GetAllAIAnalysisRequests;
using ProjektIznynierski.Application.Queries.AIAnalysisRequest.GetAIAnalysisRequestById;
using Swashbuckle.AspNetCore.Annotations;
using System.Net;
using ProjektIznynierski.Application.Commands.AIAnalysisRequest.CreateAIAnalysisRequest;

using ProjektIznynierski.Application.Commands.AIAnalysisRequest.DeleteAIAnalysisRequest;
using ProjektInzynierski.Application.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using ProjektIznynierski.Application.Queries.AIAnalysisRequest.GetAIAnalysisRequestsByClientId;

namespace ProjektIznynierski.Presentation.Controllers
{
    [Route("api/[controller]")]
    public class AIAnalysisRequestController : BaseController
    {
        private readonly IChatGPTService _chatGPTService;
        public AIAnalysisRequestController(IMediator mediator, IChatGPTService chatGPTService) : base(mediator)
        {
            _chatGPTService = chatGPTService;
        }

        [HttpGet]
        [SwaggerOperation(Summary = "Get AI Analysis Requests", Description = "Retrieves a list of all AI analysis requests.")]
        [ProducesResponseType(typeof(IEnumerable<AIAnalysisRequestDto>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> Get()
        {
            var result = await _mediator.Send(new GetAllAIAnalysisRequestsQuery());
            return Ok(result);
        }

        [HttpGet("{id}")]
        [SwaggerOperation(Summary = "Get AI Analysis Request by ID", Description = "Retrieves a specific AI analysis request by its ID.")]
        [ProducesResponseType(typeof(AIAnalysisRequestDto), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _mediator.Send(new GetAIAnalysisRequestByIdQuery(id));
            return Ok(result);
        }

        [HttpGet("my-requests/{clientId}")]
        [SwaggerOperation(Summary = "Get AI Analysis Requests by Client ID",Description = "Retrieves AI analysis requests for a specific client.")]
        [ProducesResponseType(typeof(List<AIAnalysisRequestDto>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetByClientId(int clientId)
        {
            var result = await _mediator.Send(new GetAIAnalysisRequestsByClientIdQuery(clientId));

            return Ok(result);
        }

        [HttpPost]
        [SwaggerOperation(Summary = "Create a new Analysis Request", Description = "Creates a new Analysis Request with the provided details.")]
        [ProducesResponseType(typeof(AIAnalysisRequestDto), (int)HttpStatusCode.Created)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> Create([FromBody] CreateAIAnalysisRequestCommand command)
        {
            var result = await _mediator.Send(command);
            return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
        }


        [HttpDelete("{id}")]
        [SwaggerOperation(Summary = "Delete a Analysis Request", Description = "Deletes a specific client by their ID.")]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> Delete(int id)
        {
            await _mediator.Send(new DeleteAIAnalysisRequestCommand { Id = id });
            return NoContent();
        }

    }
}
