using MediatR;
using Microsoft.AspNetCore.Mvc;
using ProjektIznynierski.Application.Dtos;
using ProjektIznynierski.Application.Queries.FinancialMetric.GetAllFinancialMetrics;
using ProjektIznynierski.Application.Queries.FinancialMetric.GetFinancialMetricById;
using Swashbuckle.AspNetCore.Annotations;
using System.Net;

namespace ProjektIznynierski.Presentation.Controllers
{
    [Route("api/[controller]")]
    public class FinancialMetricController : BaseController
    {
        public FinancialMetricController(IMediator mediator) : base(mediator)
        {
        }

        [HttpGet]
        [SwaggerOperation(Summary = "Get Financial Metrics", Description = "Retrieves a list of all financial metrics.")]
        [ProducesResponseType(typeof(IEnumerable<FinancialMetricDto>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> Get()
        {
            var result = await _mediator.Send(new GetAllFinancialMetricsQuery());
            return Ok(result);
        }

        [HttpGet("{id}")]
        [SwaggerOperation(Summary = "Get Financial Metric by ID", Description = "Retrieves a specific financial metric by its ID.")]
        [ProducesResponseType(typeof(FinancialMetricDto), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _mediator.Send(new GetFinancialMetricByIdQuery(id));
            return Ok(result);
        }
    }
}
