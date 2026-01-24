using MediatR;
using Microsoft.AspNetCore.Mvc;
using ProjektIznynierski.Application.Commands.FinancialMetric.CreateFinancialMetric;
using ProjektIznynierski.Application.Commands.FinancialMetric.DeleteFinancialMetric;
using ProjektIznynierski.Application.Commands.FinancialMetric.ImportFinancialIndicators;
using ProjektIznynierski.Application.Commands.FinancialMetric.UpdateFinancialMetric;
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
        [HttpPost()]
        [SwaggerOperation( Summary = "Create Financial Metric and assign to Invest Instrument", Description = "Creates a Financial Metric and assigns it to the selected Invest Instrument.")]
        [ProducesResponseType(typeof(int), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> CreateAndAssign([FromBody] CreateFinancialMetricAndAssignToInstrumentCommand command)
        {
            var metricId = await _mediator.Send(command);
            return Ok(metricId);
        }

        [HttpPost("import")]
        [SwaggerOperation(Summary = "Import financial indicators",Description = "Imports financial indicators from Strefa Inwestorów and creates or updates the Financial Metric.")]
        [ProducesResponseType(typeof(int), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> Import([FromBody] ImportFinancialMetricCommand command)
        {
            var metricId = await _mediator.Send(command);
            return Ok(metricId);
        }

        [HttpPut("{id}")]
        [SwaggerOperation(Summary = "Update an existing Financial Metric", Description = "Updates an existing Financial Metric with the provided details.")]
        [ProducesResponseType(typeof(FinancialMetricDto), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateFinancialMetricCommand command)
        {
            if (id != command.Id)
            {
                return BadRequest("ID in the URL does not match the ID in the request body.");
            }

            var result = await _mediator.Send(command);
            return Ok(result);
        }

        [HttpDelete("{id}")]
        [SwaggerOperation(Summary = "Delete a Financial Metric", Description = "Deletes a specific Financial Metric by its ID.")]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> Delete(int id)
        {
            await _mediator.Send(new DeleteFinancialMetricCommand { Id = id });
            return NoContent();
        }
    }
}
