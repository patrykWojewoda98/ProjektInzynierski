using MediatR;
using Microsoft.AspNetCore.Mvc;
using ProjektIznynierski.Application.Dtos;
using ProjektIznynierski.Application.Queries.AIAnalysisRequest.GetAllAIAnalysisRequests;
using ProjektIznynierski.Application.Queries.AIAnalysisRequest.GetAIAnalysisRequestById;
using Swashbuckle.AspNetCore.Annotations;
using System.Net;

namespace ProjektIznynierski.Presentation.Controllers
{
    [Route("api/[controller]")]
    public class AIAnalysisRequestController : BaseController
    {
        public AIAnalysisRequestController(IMediator mediator) : base(mediator)
        {
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
    }
}
