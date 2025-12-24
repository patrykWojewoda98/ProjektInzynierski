using MediatR;
using Microsoft.AspNetCore.Mvc;
using ProjektIznynierski.Application.Dtos;
using ProjektIznynierski.Application.Queries.AIAnalysisRequest.GetAllAIAnalysisRequests;
using ProjektIznynierski.Application.Queries.AIAnalysisRequest.GetAIAnalysisRequestById;
using Swashbuckle.AspNetCore.Annotations;
using System.Net;
using ProjektIznynierski.Application.Commands.AIAnalysisRequest.CreateAIAnalysisRequest;
using ProjektIznynierski.Application.Commands.AIAnalysisRequest.UpdateAIAnalysisRequest;
using ProjektIznynierski.Application.Commands.AIAnalysisRequest.DeleteAIAnalysisRequest;
using ProjektInzynierski.Application.Interfaces;

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

        [HttpPost]
        [SwaggerOperation(Summary = "Create a new Analysis Request", Description = "Creates a new Analysis Request with the provided details.")]
        [ProducesResponseType(typeof(AIAnalysisRequestDto), (int)HttpStatusCode.Created)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> Create([FromBody] CreateAIAnalysisRequestCommand command)
        {
            var result = await _mediator.Send(command);
            return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
        }

        [HttpPut("{id}")]
        [SwaggerOperation(Summary = "Update an existing Analysis Request", Description = "Updates an existing Analysis Request with the provided details.")]
        [ProducesResponseType(typeof(AIAnalysisRequestDto), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateAIAnalysisRequestCommand command)
        {
            if (id != command.Id)
            {
                return BadRequest("ID in the URL does not match the ID in the request body.");
            }

            var result = await _mediator.Send(command);
            return Ok(result);
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


       
        [HttpPost("ask-ai")]
        [SwaggerOperation(
            Summary = "Ask AI a question",
            Description = "Sends a message to OpenAI GPT model and returns its response."
        )]
        [ProducesResponseType(typeof(object), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> AskAI([FromBody] AIMessageRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.Message))
                return BadRequest("Message cannot be empty.");

            try
            {
                var aiResponse = await _chatGPTService.GetResponseAsync(request.Message);

                return Ok(new
                {
                    prompt = request.Message,
                    response = aiResponse
                });
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError,
                    new { error = "Error while communicating with OpenAI API", details = ex.Message });
            }
        }
    }
    /// <summary>
    /// DTO do wysy³ania wiadomoœci do ChatGPT
    /// </summary>
    public class AIMessageRequest
    {
        public string Message { get; set; } = string.Empty;
    }
}
